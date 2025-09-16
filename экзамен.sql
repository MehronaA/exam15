create table customers(
customerId serial primary key,
name varchar(150),
email varchar(100) not null unique,
phoneNumber varchar(10) not null unique,
address varchar(100) not null,
profilePhoto varchar(100)
);
create table accounts(
accountNumber int,
accountType varchar(100) check(accountType in('savings','checking')),
balance decimal check (balance>0)
);

INSERT INTO customers (name, email, phoneNumber, address, profilePhoto)
VALUES
('Алексей Иванов', 'alexey.ivanov@example.com', '9012345678', 'г. Душанбе, ул. Рудаки, 15', 'ivanov.jpg'),
('Мария Петрова', 'maria.petrova@example.com', '9023456789', 'г. Душанбе, ул. Айни, 22', 'petrova.png'),
('Дмитрий Сидоров', 'dmitry.sidorov@example.com', '9034567890', 'г. Худжанд, ул. Фирдавси, 10', 'sidorov.jpeg'),
('Екатерина Смирнова', 'ekaterina.smirnova@example.com', '9045678901', 'г. Душанбе, ул. Сино, 5', 'smirnova.jpg'),
('Иван Кузнецов', 'ivan.kuznetsov@example.com', '9056789012', 'г. Курган-Тюбе, ул. Бохтар, 18', 'kuznetsov.png');
INSERT INTO accounts (accountNumber, accountType, balance)
VALUES
(1001, 'savings', 2500.75),
(1002, 'checking', 1500.00),
(1003, 'savings', 8750.20),
(1004, 'checking', 4320.40),
(1005, 'savings', 12999.99);

