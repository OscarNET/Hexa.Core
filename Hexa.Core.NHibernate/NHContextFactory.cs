﻿#region License

//===================================================================================
//Copyright 2010 HexaSystems Corporation
//===================================================================================
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//http://www.apache.org/licenses/LICENSE-2.0
//===================================================================================
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
//===================================================================================

#endregion

using System;
using System.Reflection;
using System.Linq;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Tool.hbm2ddl;

using Hexa.Core.Database;

namespace Hexa.Core.Domain
{
    public sealed class NHContextFactory : IUnitOfWorkFactory, IDatabaseManager
    {
        private static ISessionFactory _sessionFactory;
        private static Configuration _builtConfiguration;
        private static DbProvider _DbProvider;
        private static string _connectionString;
        private static bool _InMemoryDatabase;

        public NHContextFactory(DbProvider provider, string connectionString, string cacheProvider, Assembly mappingsAssembly, IoCContainer container)
        {
            if (_sessionFactory == null)
            {
                _DbProvider = provider;
                _connectionString = connectionString;

                FluentConfiguration cfg = null;

                switch (_DbProvider)
                {
                    case DbProvider.MsSqlProvider:
                    {
                        cfg = Fluently.Configure().Database(MsSqlConfiguration.MsSql2008
                            .Raw("format_sql", "true")
                            .ConnectionString(_connectionString))
                            .ExposeConfiguration(c => c.Properties.Add(NHibernate.Cfg.Environment.SqlExceptionConverter, typeof(SqlExceptionHandler).AssemblyQualifiedName));

                        break;
                    }
                    case DbProvider.SQLiteProvider:
                    {
                        cfg = Fluently.Configure().Database(SQLiteConfiguration.Standard
                            .Raw("format_sql", "true")
                            .ConnectionString(_connectionString));
                        break;
                    }
                }

                _InMemoryDatabase = (provider == DbProvider.SQLiteProvider && _connectionString.ToUpperInvariant().Contains(":MEMORY:"));

                Guard.IsNotNull(cfg, string.Format("Db provider {0} is currently not supported.", _DbProvider.GetEnumMemberValue()));

                var pinfo = typeof(FluentConfiguration)
                    .GetProperty("Configuration", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                var nhConfiguration = pinfo.GetValue(cfg, null);
                container.RegisterInstance<Configuration>(nhConfiguration);

                cfg.Mappings(m => m.FluentMappings.Conventions.AddAssembly(typeof(NHContextFactory).Assembly))
                    .Mappings(m => m.FluentMappings.Conventions.AddAssembly(mappingsAssembly))
                    .Mappings(m => m.FluentMappings.AddFromAssembly(mappingsAssembly))
                    .Mappings(m => m.HbmMappings.AddFromAssembly(mappingsAssembly))
					.ExposeConfiguration(c => c.Properties.Add(NHibernate.Cfg.Environment.BatchSize, "100"))
                    .ExposeConfiguration(c => c.Properties.Add(NHibernate.Cfg.Environment.UseProxyValidator, "true"))
                    .ExposeConfiguration(c => c.Properties.Add(NHibernate.Cfg.Environment.DefaultSchema, "dbo"))
                    .ExposeConfiguration(c => c.Properties.Add(NHibernate.Cfg.Environment.GenerateStatistics, "true"));

                if (!string.IsNullOrEmpty(cacheProvider))
                {
                    cfg.ExposeConfiguration(c => c.Properties.Add(NHibernate.Cfg.Environment.CacheProvider, cacheProvider)) //"NHibernate.Cache.HashtableCacheProvider"
                        .ExposeConfiguration(c => c.Properties.Add(NHibernate.Cfg.Environment.UseSecondLevelCache, "true"))
                        .ExposeConfiguration(c => c.Properties.Add(NHibernate.Cfg.Environment.UseQueryCache, "true"));
                }

                _builtConfiguration = cfg.BuildConfiguration();
                _builtConfiguration.SetProperty(NHibernate.Cfg.Environment.ProxyFactoryFactoryClass, 
                    typeof(NHibernate.ByteCode.Castle.ProxyFactoryFactory).AssemblyQualifiedName);

                #region Add Validation Listeners to NHibernate pipeline....

                _builtConfiguration.SetListeners(ListenerType.PreInsert,
                    _builtConfiguration.EventListeners.PreInsertEventListeners.Concat<IPreInsertEventListener>(
                    new IPreInsertEventListener[] { new ValidateEventListener(), new AuditEventListener() }).ToArray());

                _builtConfiguration.SetListeners(ListenerType.PreUpdate,
                    _builtConfiguration.EventListeners.PreUpdateEventListeners.Concat<IPreUpdateEventListener>(
                    new IPreUpdateEventListener[] { new ValidateEventListener(), new AuditEventListener() }).ToArray());

                #endregion
            }
        }

        private void _CreateSessionFactory()
        {
            if (_sessionFactory == null)
                _sessionFactory = _builtConfiguration.BuildSessionFactory();
        }

        public IUnitOfWork Create()
        {
            _CreateSessionFactory();
            
            var session = _sessionFactory.OpenSession();

            if (_InMemoryDatabase)
                new SchemaExport(_builtConfiguration).Execute(false, true, false, session.Connection, Console.Out);

            return new NHibernateContext(session);
        }

        // Registers NH session factoy for testing purposes.
        public void RegisterSessionFactory(IoCContainer container)
        {
            _CreateSessionFactory();
            container.RegisterInstance<ISessionFactory>(_sessionFactory);
        }

        public bool DatabaseExists()
        {
            var dbManager = new DatabaseManager(_DbProvider, _connectionString);
            return dbManager.DatabaseExists();
        }

        public void CreateDatabase()
        {
            var dbManager = new DatabaseManager(_DbProvider, _connectionString);
            
            // Check if database exists.. (and create it if needed)
            if (!dbManager.DatabaseExists())
            {
                dbManager.CreateDatabase();
                new SchemaExport(_builtConfiguration).Create(true, true);
            }
        }

        public void ValidateDatabaseSchema()
        {
            if (!_InMemoryDatabase)
                new SchemaValidator(_builtConfiguration).Validate();
        }

        public void DeleteDatabase()
        {
            var dbManager = new DatabaseManager(_DbProvider, _connectionString);

            if (dbManager.DatabaseExists())
                dbManager.DropDatabase();
        }
    }
}
