
using Microsoft.EntityFrameworkCore;
using ProjetoPendencia.Api.Controllers;
using ProjetoPendencia.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TestePendencia
{
    public class ClientUnitTest
    {
        private DbContextOptions<PendenciaContext> options;

        private void InitializeDataBase()
        {
            // Create a Temporary Database
            options = new DbContextOptionsBuilder<PendenciaContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Insert data into the database using one instance of the context
            using (var context = new PendenciaContext(options))
            {
                context.Pendencia.Add(new Pendencia { Id = 1, Descricao = "Name 1", Data = "22/01/2021" });
                context.Pendencia.Add(new Pendencia { Id = 2, Descricao = "Name 2", Data = "22/01/2021" });
                context.Pendencia.Add(new Pendencia { Id = 3, Descricao = "Name 3", Data = "22/01/2021" });
                context.SaveChanges();
            }
        }

        [Fact]
        public void GetAll()
        {
            InitializeDataBase();

            // Use a clean instance of the context to run the test
            using (var context = new PendenciaContext(options))
            {
                PendenciaController pendenciaController = new PendenciaController(context);
                IEnumerable<Pendencia> pendencias = pendenciaController.GetPendencia().Result.Value;

                Assert.Equal(3, pendencias.Count());
            }
        }

        [Fact]
        public void GetbyId()
        {
            InitializeDataBase();

            // Use a clean instance of the context to run the test
            using (var context = new PendenciaContext(options))
            {
                int pendenciaId = 2;
                PendenciaController pendenciaController = new PendenciaController(context);
                Pendencia pendencia = pendenciaController.GetPendencia(pendenciaId).Result.Value;
                Assert.Equal(2, pendencia.Id);
            }
        }

        [Fact]
        public void Create()
        {
            InitializeDataBase();

            Pendencia pendencia = new Pendencia()
            {
                Id = 4,
                Descricao = "Name 4",
                Data = "22/01/2020"
            };

            // Use a clean instance of the context to run the test
            using (var context = new PendenciaContext(options))
            {
                PendenciaController pendenciaController = new PendenciaController(context);
                Pendencia pen = pendenciaController.PostPendencia(pendencia).Result.Value;
                Assert.Equal(4, pen.Id);
            }
        }

        [Fact]
        public void Update()
        {
            InitializeDataBase();

            Pendencia pendencia = new Pendencia()
            {
                Id = 3,
                Descricao = "Name 5",
                Data = "22/01/2020"
            };

            // Use a clean instance of the context to run the test
            using (var context = new PendenciaContext(options))
            {
                PendenciaController pendenciaController = new PendenciaController(context);
                Pendencia pen = pendenciaController.PutPendencia(3, pendencia).Result.Value;
                Assert.Equal("Name 5", pen.Descricao);
            }
        }

        [Fact]
        public void Delete()
        {
            InitializeDataBase();

            // Use a clean instance of the context to run the test
            using (var context = new PendenciaContext(options))
            {
                PendenciaController pendenciaController = new PendenciaController(context);
                Pendencia pen = pendenciaController.DeletePendencia(2).Result.Value;
                Assert.Null(pen);
            }
        }
    }
}