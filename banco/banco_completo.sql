CREATE DATABASE item_personalizados;
USE item_personalizados;

CREATE TABLE perfil_usuario(
	id INT PRIMARY KEY AUTO_INCREMENT,
	nome_perfil VARCHAR(15) NOT NULL UNIQUE
);

CREATE TABLE usuario (
	id INT PRIMARY KEY AUTO_INCREMENT,
	perfil_usuario_id INT NOT NULL,
	email VARCHAR(50) NOT NULL UNIQUE,
	senha VARCHAR(255) NOT NULL,
	status_conta BOOLEAN NOT NULL,
	CONSTRAINT fk_usuario_perfil_usuario FOREIGN KEY (perfil_usuario_id)
		REFERENCES perfil_usuario(id)
);

CREATE TABLE funcionario(
	id int PRIMARY KEY AUTO_INCREMENT,
	usuario_id INT NOT NULL UNIQUE,
	nome VARCHAR(100) NOT NULL,
	cpf VARCHAR(11) NOT NULL UNIQUE,
	CONSTRAINT FK_usuario_funcionario FOREIGN KEY (usuario_id)
		REFERENCES usuario(id)
);

CREATE TABLE cliente(
	id INT PRIMARY KEY AUTO_INCREMENT,
	usuario_id INT NOT NULL UNIQUE,
	nome VARCHAR(100) NOT NULL,
	cpf VARCHAR(11) UNIQUE NOT NULL,
	telefone VARCHAR(11) NOT NULL,
	CONSTRAINT FK_cliente_usuario FOREIGN KEY (usuario_id)
		REFERENCES usuario(id)
);

CREATE TABLE pedido(
	id INT PRIMARY KEY AUTO_INCREMENT,
	cliente_id INT NOT NULL,
	data_pedido DATE NOT NULL,
	prazo_entrega DATE NOT NULL,
	status_pedido VARCHAR(20) NOT NULL,
	valor_total DECIMAL(10,2) NOT NULL,
	CONSTRAINT FK_pedido_cliente FOREIGN KEY (cliente_id)
		REFERENCES cliente(id)
);

CREATE TABLE categoria_produto(
	id int PRIMARY KEY AUTO_INCREMENT,
	nome_categoria VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE produto(
	id int PRIMARY KEY AUTO_INCREMENT,
	categoria_produto_id INT NOT NULL,
	nome_produto VARCHAR(50) NOT NULL,
	material VARCHAR(50) NOT NULL,
	dimensao VARCHAR(50) NOT NULL,
	preco DECIMAL(10,2),
	CONSTRAINT FK_produto_categoria_produto FOREIGN KEY (categoria_produto_id)
		REFERENCES categoria_produto(id)
);

CREATE TABLE item_pedido(
	id INT PRIMARY KEY AUTO_INCREMENT,
	pedido_id INT NOT NULL,
	produto_id INT NOT NULL,
	quantidade INT NOT NULL,
	preco_unitario DECIMAL(10,2) NOT NULL,
	CONSTRAINT FK_item_pedido_pedido FOREIGN KEY (pedido_id)
		REFERENCES pedido(id),
	CONSTRAINT FK_item_pedido_produto FOREIGN KEY (produto_id)
		REFERENCES produto(id)
);

-- ============================================
-- Povoamento: item_personalizados
USE item_personalizados;

INSERT INTO perfil_usuario (nome_perfil) VALUES ('Cliente'), ('Funcionário'), ('Gerente');

-- Senhas armazenadas como hash SHA-256 (dados fictícios)
INSERT INTO usuario (perfil_usuario_id, email, senha, status_conta) VALUES
(1, 'maria.silva@email.com', SHA2('senha1', 256), TRUE),
(1, 'joao.pereira@email.com', SHA2('senha2', 256), TRUE),
(1, 'ana.souza@email.com', SHA2('senha3', 256), TRUE),
(1, 'carlos.lima@email.com', SHA2('senha4', 256), TRUE),
(1, 'fernanda.costa@email.com', SHA2('senha5', 256), TRUE);

INSERT INTO cliente (usuario_id, nome, cpf, telefone) VALUES
(1, 'Maria Silva', '52998224725', '11987654321'),
(2, 'João Pereira', '11144477735', '21976543210'),
(3, 'Ana Souza', '39053344705', '31965432109'),
(4, 'Carlos Lima', '86288366757', '41954321098'),
(5, 'Fernanda Costa', '74682489000', '51943210987');

INSERT INTO categoria_produto (nome_categoria) VALUES
('Almofada'),
('Almochaveiro'),
('Caneca'),
('Camiseta'),
('Copo'),
('Chaveiro'),
('Caixa'),
('Taça');

INSERT INTO produto (categoria_produto_id, nome_produto, material, dimensao, preco) VALUES
(1, 'Almofada', 'Poliéster', '60x60cm', 59.90),
(1, 'Almofada', 'Poliéster', '60x40cm', 49.90),
(1, 'Almofada', 'Poliéster', '50x25cm', 39.90),
(2, 'Almochaveiro', 'Poliéster', '10x7cm', 12.90),
(2, 'Almochaveiro', 'Poliéster', '7x5cm', 9.90),
(3, 'Caneca de cerâmica', 'Cerâmica', 'Padrão', 29.90),
(3, 'Caneca para chopp', 'Acrílico', 'Padrão', 34.90),
(4, 'Camiseta', 'Poliéster', 'PP', 39.90),
(4, 'Camiseta', 'Poliéster', 'P', 39.90),
(4, 'Camiseta', 'Poliéster', 'M', 39.90),
(4, 'Camiseta', 'Poliéster', 'G', 39.90),
(4, 'Camiseta', 'Poliéster', 'GG', 42.90),
(4, 'Camiseta', 'Poliéster', 'EGG', 44.90),
(5, 'Copo Long Drink', 'Acrílico', 'Padrão', 24.90),
(5, 'Copo Twister', 'Acrílico', 'Padrão', 22.90),
(6, 'Chaveiro', 'Acrílico', '7x5cm', 8.90),
(7, 'Caixa cartonada', 'Papel cartão', '35x25x10cm', 7.50),
(7, 'Caixa cartonada', 'Papel cartão', '22x16x9cm', 5.50),
(7, 'Caixa cartonada', 'Papel cartão', '18x18x7cm', 4.50),
(7, 'Caixa cartonada', 'Papel cartão', '12,5x12,5x5cm', 3.50),
(8, 'Taça de Gin', 'Acrílico', 'Padrão', 27.90),
(8, 'Taça de Champanhe', 'Acrílico', 'Padrão', 25.90);

INSERT INTO pedido (cliente_id, data_pedido, prazo_entrega, status_pedido, valor_total) VALUES
(1, '2026-10-05', '2026-10-19', 'Entregue', 748.20),
(2, '2026-10-12', '2026-10-26', 'Entregue', 1860.50),
(3, '2026-10-20', '2026-11-06', 'Em produção', 2393.00),
(4, '2026-10-27', '2026-11-10', 'Pronto para retirada', 873.00),
(5, '2026-11-01', '2026-11-16', 'Aguardando pagamento', 831.80);

INSERT INTO item_pedido (pedido_id, produto_id, quantidade, preco_unitario) VALUES
(1, 1, 4, 59.90),
(1, 2, 2, 49.90),
(1, 3, 2, 39.90),
(1, 6, 10, 29.90),
(1, 17, 4, 7.50),
(2, 8, 5, 39.90),
(2, 9, 10, 39.90),
(2, 10, 10, 39.90),
(2, 11, 5, 39.90),
(2, 12, 3, 42.90),
(2, 13, 2, 44.90),
(2, 16, 50, 8.90),
(3, 4, 30, 12.90),
(3, 5, 30, 9.90),
(3, 7, 20, 34.90),
(3, 14, 20, 24.90),
(3, 15, 20, 22.90),
(3, 18, 10, 5.50),
(4, 21, 12, 27.90),
(4, 22, 12, 25.90),
(4, 19, 6, 4.50),
(4, 20, 6, 3.50),
(4, 6, 6, 29.90),
(5, 3, 3, 39.90),
(5, 10, 8, 39.90),
(5, 16, 25, 8.90),
(5, 17, 2, 7.50),
(5, 22, 6, 25.90);
