using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PrototipoSiteGestaoArtesBujujus.Entidades;

namespace PrototipoSiteGestaoArtesBujujus.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoriaProduto> CategoriaProdutos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Funcionario> Funcionarios { get; set; }

    public virtual DbSet<ItemPedido> ItemPedidos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<PerfilUsuario> PerfilUsuarios { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_uca1400_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<CategoriaProduto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("categoria_produto");

            entity.HasIndex(e => e.NomeCategoria, "nome_categoria").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.NomeCategoria)
                .HasMaxLength(50)
                .HasColumnName("nome_categoria");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cliente");

            entity.HasIndex(e => e.Cpf, "cpf").IsUnique();

            entity.HasIndex(e => e.UsuarioId, "usuario_id").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .HasColumnName("cpf");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Telefone)
                .HasMaxLength(11)
                .HasColumnName("telefone");
            entity.Property(e => e.UsuarioId)
                .HasColumnType("int(11)")
                .HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithOne(p => p.Cliente)
                .HasForeignKey<Cliente>(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cliente_usuario");
        });

        modelBuilder.Entity<Funcionario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("funcionario");

            entity.HasIndex(e => e.Cpf, "cpf").IsUnique();

            entity.HasIndex(e => e.UsuarioId, "usuario_id").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .HasColumnName("cpf");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.UsuarioId)
                .HasColumnType("int(11)")
                .HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithOne(p => p.Funcionario)
                .HasForeignKey<Funcionario>(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_usuario_funcionario");
        });

        modelBuilder.Entity<ItemPedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("item_pedido");

            entity.HasIndex(e => e.PedidoId, "FK_item_pedido_pedido");

            entity.HasIndex(e => e.ProdutoId, "FK_item_pedido_produto");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.PedidoId)
                .HasColumnType("int(11)")
                .HasColumnName("pedido_id");
            entity.Property(e => e.PrecoUnitario)
                .HasPrecision(10, 2)
                .HasColumnName("preco_unitario");
            entity.Property(e => e.ProdutoId)
                .HasColumnType("int(11)")
                .HasColumnName("produto_id");
            entity.Property(e => e.Quantidade)
                .HasColumnType("int(11)")
                .HasColumnName("quantidade");

            entity.HasOne(d => d.Pedido).WithMany(p => p.ItemPedidos)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_item_pedido_pedido");

            entity.HasOne(d => d.Produto).WithMany(p => p.ItemPedidos)
                .HasForeignKey(d => d.ProdutoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_item_pedido_produto");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pedido");

            entity.HasIndex(e => e.ClienteId, "FK_pedido_cliente");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.ClienteId)
                .HasColumnType("int(11)")
                .HasColumnName("cliente_id");
            entity.Property(e => e.DataPedido).HasColumnName("data_pedido");
            entity.Property(e => e.PrazoEntrega).HasColumnName("prazo_entrega");
            entity.Property(e => e.StatusPedido)
                .HasMaxLength(20)
                .HasColumnName("status_pedido");
            entity.Property(e => e.ValorTotal)
                .HasPrecision(10, 2)
                .HasColumnName("valor_total");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_pedido_cliente");
        });

        modelBuilder.Entity<PerfilUsuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("perfil_usuario");

            entity.HasIndex(e => e.NomePerfil, "nome_perfil").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.NomePerfil)
                .HasMaxLength(15)
                .HasColumnName("nome_perfil");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("produto");

            entity.HasIndex(e => e.CategoriaProdutoId, "FK_produto_categoria_produto");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CategoriaProdutoId)
                .HasColumnType("int(11)")
                .HasColumnName("categoria_produto_id");
            entity.Property(e => e.Dimensao)
                .HasMaxLength(50)
                .HasColumnName("dimensao");
            entity.Property(e => e.Material)
                .HasMaxLength(50)
                .HasColumnName("material");
            entity.Property(e => e.NomeProduto)
                .HasMaxLength(50)
                .HasColumnName("nome_produto");
            entity.Property(e => e.Preco)
                .HasPrecision(10, 2)
                .HasColumnName("preco");

            entity.HasOne(d => d.CategoriaProduto).WithMany(p => p.Produtos)
                .HasForeignKey(d => d.CategoriaProdutoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_produto_categoria_produto");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.PerfilUsuarioId, "fk_usuario_perfil_usuario");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.PerfilUsuarioId)
                .HasColumnType("int(11)")
                .HasColumnName("perfil_usuario_id");
            entity.Property(e => e.Senha)
                .HasMaxLength(255)
                .HasColumnName("senha");
            entity.Property(e => e.StatusConta).HasColumnName("status_conta");

            entity.HasOne(d => d.PerfilUsuario).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.PerfilUsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_perfil_usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
