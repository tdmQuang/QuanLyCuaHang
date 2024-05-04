CREATE DATABASE QLCH
GO

USE QLCH
GO

CREATE TABLE TableFood (
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) not null DEFAULT N'Chưa đặt tên!',
	status NVARCHAR(100) not null DEFAULT N'Trống'
)
GO

CREATE TABLE Account
(
	Username NVARCHAR(100) PRIMARY KEY, 
	DisplayName NVARCHAR(100) not null DEFAULT N'abc',
	Password NVARCHAR(1000) not null DEFAULT 0,
	Type INT not null DEFAULT 0
)
GO
CREATE TABLE FoodCategory (
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) not null DEFAULT N'Chưa đặt tên!',

)
GO
CREATE TABLE Food (
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) not null DEFAULT N'Chưa đặt tên!',
	idCategory INT not null,
	price FLOAT not null DEFAULT 0, 

	FOREIGN KEY (idCategory) REFERENCES dbo.FoodCategory(id)
)
GO

CREATE TABLE Bill (
	id INT IDENTITY PRIMARY KEY,
	DataCheckIn DATE not null DEFAULT GETDATE(),
	DataCheckOut DATE not null,
	idTable INT not null,
	status INT not null DEFAULT 0,

	FOREIGN KEY (idTable) REFERENCES dbo.TableFood(id)
)
GO

CREATE TABLE BillInfo (
	id INT IDENTITY PRIMARY KEY,
	idBill INT not null,
	idFood INT not null,
	count INT not null DEFAULT 0,

	FOREIGN KEY (idBill) REFERENCES dbo.Bill(id),
	FOREIGN KEY (idFood) REFERENCES dbo.Food(id)
)
GO


insert into dbo.Account(Username, DisplayName, Password, Type) values(N'abc', N'abc1', N'1', 1)
insert into dbo.Account(Username, DisplayName, Password, Type) values(N'staff', N'staff', N'1', 0)
go

create proc USP_GetAccountByUsername
@userName nvarchar(100)
as
begin
	select * from dbo.Account where Username = @userName
end
go

exec dbo.USP_GetAccountByUsername @userName = N'abc' --nvarchar(100)

select * from dbo.Account





