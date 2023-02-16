﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BotTelegramNET.Models;

namespace BotTelegramNET.Context
{
    public partial class SistemaDeGerenciamento2_0Context : DbContext
    {
        public SistemaDeGerenciamento2_0Context()
        {
        }

        public SistemaDeGerenciamento2_0Context(DbContextOptions<SistemaDeGerenciamento2_0Context> options)
            : base(options)
        {
        }

        public virtual DbSet<tb_cadastro_despesa> tb_cadastro_despesa { get; set; }
        public virtual DbSet<tb_configuracao_financeira> tb_configuracao_financeira { get; set; }
        public virtual DbSet<tb_despesa> tb_despesa { get; set; }
        public virtual DbSet<tb_enderecos> tb_enderecos { get; set; }
        public virtual DbSet<tb_estoque> tb_estoque { get; set; }
        public virtual DbSet<tb_grupo> tb_grupo { get; set; }
        public virtual DbSet<tb_informacoes_comerciais> tb_informacoes_comerciais { get; set; }
        public virtual DbSet<tb_nota_fiscal_entrada> tb_nota_fiscal_entrada { get; set; }
        public virtual DbSet<tb_nota_fiscal_saida> tb_nota_fiscal_saida { get; set; }
        public virtual DbSet<tb_permissoes> tb_permissoes { get; set; }
        public virtual DbSet<tb_produto> tb_produto { get; set; }
        public virtual DbSet<tb_registro> tb_registro { get; set; }
        public virtual DbSet<tb_repeticao_despesa> tb_repeticao_despesa { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=ISRAEL\\SQLEXPRESS;Initial Catalog=SistemaDeGerenciamento2_0;Persist Security Info=True;User ID=sa;Password=12345");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tb_cadastro_despesa>(entity =>
            {
                entity.HasKey(e => e.id_categoria_despesa)
                    .HasName("PK_TB_CADASTRO_DESPESA");
            });

            modelBuilder.Entity<tb_configuracao_financeira>(entity =>
            {
                entity.HasKey(e => e.id_configuracao_financeira)
                    .HasName("PK_TB_CONFIGURACAO_FINANCEIRA");

                entity.HasOne(d => d.fk_grupoNavigation)
                    .WithMany(p => p.tb_configuracao_financeira)
                    .HasForeignKey(d => d.fk_grupo)
                    .HasConstraintName("tb_configuracao_financeira_fk0");
            });

            modelBuilder.Entity<tb_despesa>(entity =>
            {
                entity.HasKey(e => e.id_despesa)
                    .HasName("PK_TB_DESPESA");

                entity.HasOne(d => d.fk_registroNavigation)
                    .WithMany(p => p.tb_despesa)
                    .HasForeignKey(d => d.fk_registro)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_despesa_fk0");

                entity.HasOne(d => d.fk_repeticao_despesaNavigation)
                    .WithMany(p => p.tb_despesa)
                    .HasForeignKey(d => d.fk_repeticao_despesa)
                    .HasConstraintName("tb_despesa_fk1");
            });

            modelBuilder.Entity<tb_enderecos>(entity =>
            {
                entity.HasKey(e => e.id_endereco)
                    .HasName("PK_TB_ENDERECOS");
            });

            modelBuilder.Entity<tb_estoque>(entity =>
            {
                entity.HasKey(e => e.id_estoque)
                    .HasName("PK_TB_ESTOQUE");

                entity.HasOne(d => d.fk_nota_fiscal_entradaNavigation)
                    .WithMany(p => p.tb_estoque)
                    .HasForeignKey(d => d.fk_nota_fiscal_entrada)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_estoque_fk1");

                entity.HasOne(d => d.fk_produtoNavigation)
                    .WithMany(p => p.tb_estoque)
                    .HasForeignKey(d => d.fk_produto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_estoque_fk0");
            });

            modelBuilder.Entity<tb_grupo>(entity =>
            {
                entity.HasKey(e => e.id_grupo)
                    .HasName("PK_TB_GRUPO");
            });

            modelBuilder.Entity<tb_informacoes_comerciais>(entity =>
            {
                entity.HasKey(e => e.id_informacao_comercial)
                    .HasName("PK_TB_INFORMACOES_COMERCIAIS");
            });

            modelBuilder.Entity<tb_nota_fiscal_entrada>(entity =>
            {
                entity.HasKey(e => e.id_nota_fiscal_entrada)
                    .HasName("PK_TB_NOTA_FISCAL_ENTRADA");
            });

            modelBuilder.Entity<tb_nota_fiscal_saida>(entity =>
            {
                entity.HasKey(e => e.id_nota_fiscal_saida)
                    .HasName("PK_TB_NOTA_FISCAL_SAIDA");

                entity.HasOne(d => d.fk_estoqueNavigation)
                    .WithMany(p => p.tb_nota_fiscal_saida)
                    .HasForeignKey(d => d.fk_estoque)
                    .HasConstraintName("tb_nota_fiscal_saida_fk0");

                entity.HasOne(d => d.fk_registro_clienteNavigation)
                    .WithMany(p => p.tb_nota_fiscal_saida)
                    .HasForeignKey(d => d.fk_registro_cliente)
                    .HasConstraintName("tb_nota_fiscal_saida_fk1");
            });

            modelBuilder.Entity<tb_permissoes>(entity =>
            {
                entity.HasKey(e => e.id_permissoes)
                    .HasName("PK_TB_PERMISSOES");
            });

            modelBuilder.Entity<tb_produto>(entity =>
            {
                entity.HasKey(e => e.id_produto)
                    .HasName("PK_TB_PRODUTO");

                entity.HasOne(d => d.fk_grupoNavigation)
                    .WithMany(p => p.tb_produto)
                    .HasForeignKey(d => d.fk_grupo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_produto_fk0");

                entity.HasOne(d => d.fk_registro_forncedorNavigation)
                    .WithMany(p => p.tb_produto)
                    .HasForeignKey(d => d.fk_registro_forncedor)
                    .HasConstraintName("tb_produto_fk1");
            });

            modelBuilder.Entity<tb_registro>(entity =>
            {
                entity.HasKey(e => e.id_registro)
                    .HasName("PK_TB_REGISTRO");

                entity.HasOne(d => d.fk_enderecoNavigation)
                    .WithMany(p => p.tb_registro)
                    .HasForeignKey(d => d.fk_endereco)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_registro_fk2");

                entity.HasOne(d => d.fk_informacao_comercialNavigation)
                    .WithMany(p => p.tb_registro)
                    .HasForeignKey(d => d.fk_informacao_comercial)
                    .HasConstraintName("tb_registro_fk1");

                entity.HasOne(d => d.fk_permissoesNavigation)
                    .WithMany(p => p.tb_registro)
                    .HasForeignKey(d => d.fk_permissoes)
                    .HasConstraintName("tb_registro_fk0");
            });

            modelBuilder.Entity<tb_repeticao_despesa>(entity =>
            {
                entity.HasKey(e => e.id_repeticao_despesas)
                    .HasName("PK_TB_REPETICAO_DESPESA");

                entity.HasOne(d => d.fk_cadastro_despesaNavigation)
                    .WithMany(p => p.tb_repeticao_despesa)
                    .HasForeignKey(d => d.fk_cadastro_despesa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_repeticao_despesa_fk0");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}