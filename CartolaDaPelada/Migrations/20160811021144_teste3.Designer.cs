﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Repositorio.Contexto;

namespace CartolaDaPelada.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20160811021144_teste3")]
    partial class teste3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("Domain.Entities.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Apelido")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<byte>("Numero");

                    b.Property<string>("Posicao")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("SobreNome")
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("Id");

                    b.ToTable("usuario");
                });
        }
    }
}