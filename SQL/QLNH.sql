CREATE DATABASE QLNH
GO

USE QLNH
GO

CREATE TABLE TableFood
(
	idTable int IDENTITY PRIMARY KEY,
	nameTable nvarchar(40),
	status int NOT NULL DEFAULT 0 -- 0 là trống, 1 là có ng
)
GO

CREATE TABLE Staff
(
	idStaff char(4) PRIMARY KEY,
	fullName nvarchar(40),
	phoneNumber varchar(10),
	firstDoW smalldatetime,
	position nvarchar(20)
)
GO
	
CREATE TABLE Account
(
	userName varchar(30) PRIMARY KEY,
	passWord varchar(256) NOT NULL DEFAULT 0,
	idStaff char(4)

	FOREIGN KEY (idStaff) REFERENCES dbo.Staff(idStaff)
)
GO

CREATE TABLE FoodCategory
(
	idCategory int IDENTITY PRIMARY KEY,
	name nvarchar(100) NOT NULL DEFAULT N'Chưa đặt tên'
)
GO	

CREATE TABLE Food
(
	idFood int IDENTITY PRIMARY KEY,
	name nvarchar(100) NOT NULL DEFAULT N'Chưa đặt tên',
	idCategory int NOT NULL,
	price FLOAT NOT NULL DEFAULT 0
	
	FOREIGN KEY (idCategory) REFERENCES dbo.FoodCategory(idCategory)
)
GO

CREATE TABLE Bill
(
	idBill int IDENTITY PRIMARY KEY,
	idStaff char(4),
	DateCheckIn smalldatetime NOT NULL DEFAULT GETDATE(),
	DateCheckOut smalldatetime,
	idTable int NOT NULL,
	total money NOT NULL DEFAULT 0,
	status int NOT NULL DEFAULT 0, -- 1: đã thanh toán && 0: chưa thanh toán
	discount int

	FOREIGN KEY (idStaff) REFERENCES dbo.Staff(idStaff),
	FOREIGN KEY (idTable) REFERENCES dbo.TableFood(idTable)
)
GO

CREATE TABLE BillInfo
(
	idBill int NOT NULL,
	idFood int NOT NULL,
	count int NOT NULL DEFAULT 0
	
	FOREIGN KEY (idBill) REFERENCES dbo.Bill(idBill),
	FOREIGN KEY (idFood) REFERENCES dbo.Food(idFood)
)
GO

SET DATEFORMAT DMY
-- Thêm dữ liệu vào bảng Staff
INSERT INTO Staff (idStaff, fullName, phoneNumber, firstDoW, position)
VALUES ('M001', N'Trương Đức Minh Quang', '0123456789', '01/05/2023', N'Quản Lý')
INSERT INTO Staff (idStaff, fullName, phoneNumber, firstDoW, position)
VALUES ('S001', N'Trịnh Vinh Đại', '0222222222', '04/06/2023', N'Nhân Viên')
INSERT INTO Staff (idStaff, fullName, phoneNumber, firstDoW, position)
VALUES ('S002', N'Huỳnh Đỗ Thiên Ân ', '0333333333', '04/05/2024', N'Nhân Viên')
INSERT INTO Staff (idStaff, fullName, phoneNumber, firstDoW, position)
VALUES ('S003', N'Lê Nguyễn Thảo Vân ', '0444444444', '04/05/2024', N'Nhân Viên')
select *from Staff

-- Thêm dữ liệu vào bảng Account, với idStaff tương ứng là 'S001'
INSERT INTO Account (userName, passWord, idStaff)
VALUES ('minhquang', '34a378e876712e095ed1059a4ee44ea2ac7c93831dd4f58e5e980853a6b7999e', 'M001')
INSERT INTO Account (userName, passWord, idStaff)
VALUES ('vinhdai', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 'S001')
INSERT INTO Account (userName, passWord, idStaff)
VALUES ('thienan', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'S002')
INSERT INTO Account (userName, passWord, idStaff)
VALUES ('thaovan', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'S003')
--DELETE FROM Account WHERE userName = 'minhquang' AND idStaff = 'M001';

-- Thêm category

INSERT dbo.FoodCategory ( name ) VALUES ( N'Gà - Vịt' ) --1
INSERT dbo.FoodCategory ( name ) VALUES ( N'Heo - Bò' ) --2
INSERT dbo.FoodCategory ( name ) VALUES ( N'Cá - Tôm - Cua - Mực' ) --3
INSERT dbo.FoodCategory ( name ) VALUES ( N'Rau' ) --4
INSERT dbo.FoodCategory ( name ) VALUES ( N'Lẩu' ) --5
INSERT dbo.FoodCategory ( name ) VALUES ( N'Nộm - Salad' ) --6
INSERT dbo.FoodCategory ( name ) VALUES ( N'Súp - Cháo' ) --7
INSERT dbo.FoodCategory ( name ) VALUES ( N'Đồ uống' ) --8

-- Thêm đồ ăn

INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Gà rang muối', 1, 295000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Gà chiên mắn', 1, 295000 )	
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Gà hấp mắm nhĩ', 1, 295000 )	
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Cánh gà chiên giòn', 1, 285000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Đùi gà chiên giòn', 1, 325000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Vịt Quay', 1, 295000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Vịt om sấu', 1, 295000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Vịt om khoai môn', 1, 295000 ) 
	
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Heo nướng', 2, 225000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Sườn chiên vị tỏi', 2, 250000 )	
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Sường nướng BBQ', 2, 280000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Chân giò chiên', 2, 365000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Bò sốt tiêu đen', 2, 280000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Bò lúc lắc trái thơm', 2, 295000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Bò xào măng trúc', 2, 225000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Bê xào lăn', 2, 200000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Bê tái chanh', 2, 225000 ) 


INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Cá lăng xào hành nấm', 3, 225000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Cá lăng rang muối', 3, 300000 )	
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Cá lăng hấp xì dầu', 3, 425000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Cá lăng om chuối đậu', 3, 395000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Cá chìn nướng', 3, 425000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Cá lăng om chuối đậu', 3, 550000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Cá tầm rang muối', 3, 450000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Cá tầm nướng muối ớt', 3, 500000 ) 	
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Cá tầm hấp xì dầu', 3, 635000 ) 	
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Tôm rang muối', 3, 290000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Tôm chiên Hoàng bào', 3, 350000 )	
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Tôm sú hấp trái dừa', 3, 485000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Mực chiên bơ', 3, 295000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Mực xào ngũ sắc', 3, 425000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Mực sốt canh dây', 3, 550000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Cua Cà Mau', 3, 450000 ) 



INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Cải chíp sốt nấm', 4, 85000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Súp lơ xanh xào tỏi', 4, 90000 )	
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Ngó xuân xào tỏi', 4, 100000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Ngồng xào tỏi', 4, 100000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Thiên lý xào tỏi', 4, 100000 ) 

INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Lẩu gà', 5, 550000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Lẩu cá lăng', 5, 425000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Lẩu Thái', 5, 475000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Lẩu Hải sản', 5, 575000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Lẩu cá tầm', 5, 635000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Lẩu nấm chim câu', 5, 635000 ) 


INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Nộp chân gà rút xương', 6, 125000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Nộp sứa ngọc trai', 6, 125000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Nộp rau má bắp bó', 6, 125000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Salad cá ngừ', 6, 135000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Salad trái cây', 6, 135000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Salad trong biển trứng cua', 6, 165000 ) 

INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Súp măng tây cua bể', 7, 185000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Súp gà nấm', 7, 185000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Súp hải sản', 7, 185000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Cháo cá lăng', 7, 185000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Cháo tôm', 7, 185000 ) 

INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Bia Saigon', 8, 12000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Bia Saigon (thùng 24 lon)', 8, 252000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Bia 333', 8, 12600 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Bia 333 (thùng 24 lon)', 8, 269000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Bia Tiger', 8, 17500 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Bia Tiger (thùng 24 lon)', 8, 340000 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Coca', 8, 10400 )	
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Pepsi', 8, 10400 ) 
INSERT dbo.Food ( name, idCategory, price ) VALUES ( N'Sprite', 8, 92000 ) 



--Insert default table
DECLARE @i INT = 1

WHILE @i <= 10
BEGIN
	INSERT TableFood ( nameTable ) VALUES ( N'Bàn ' + CAST(@i AS nvarchar(40)))
	SET @i = @i + 1
END

GO

CREATE PROC USP_InsertBill
@idTable int, @idStaff char(4)
AS
BEGIN
	INSERT dbo.Bill (idStaff, DateCheckIn, DateCheckOut, idTable, status, discount ) VALUES (@idStaff, GETDATE(), NULL, @idTable, 0, 0)
	UPDATE dbo.TableFood SET status =  1 WHERE idTable = @idTable
END
GO

GO
CREATE PROC USP_InsertBillInfo
@idBill int, @idFood int, @count int
AS
BEGIN
	DECLARE @foodCount int = 1;
	IF NOT EXISTS (SELECT idBill FROM dbo.BillInfo WHERE idBill = @idBill)
	BEGIN
		INSERT dbo.BillInfo ( idBill, idFood, count ) VALUES ( @idBill, @idFood, @count )
	END
	ELSE
	BEGIN
		SELECT @foodCount = count FROM dbo.BillInfo WHERE idBill = @idBill AND idFood = @idFood
		IF NOT EXISTS (SELECT idFood FROM dbo.BillInfo WHERE idBill = @idBill AND idFood = @idFood)
		BEGIN
			INSERT dbo.BillInfo ( idBill, idFood, count ) VALUES ( @idBill, @idFood, @count )
		END
		ELSE
		BEGIN
			DECLARE @newCount int = @foodCount + @count
			IF (@newCount > 0)
				UPDATE dbo.BillInfo SET count = @newCount WHERE idBill = @idBill AND idFood = @idFood
			ELSE
				DELETE dbo.BillInfo WHERE idBill = @idBill AND idFood = @idFood
		END
	END
END

-- Proc CheckOut
GO
CREATE PROC USP_CheckOut
@idBill int, @discount int, @total money
AS
BEGIN
	DECLARE @idTable int;
	SELECT @idTable = idTable FROM dbo.Bill WHERE idBill = @idBill
	UPDATE dbo.Bill SET DateCheckOut = GETDATE(), status = 1, discount = @discount, total = @total WHERE idBill = @idBill
	UPDATE dbo.TableFood SET status =  0 WHERE idTable = @idTable
END
GO

----update acc proc
GO
CREATE PROC USP_UpdateAccount
@idStaff char(4) , @fullName nvarchar(40) , @phoneNumber varchar(10) , @userName varchar(30) , @passWord varchar(256) , @newPassWord varchar(256)
AS
BEGIN	
	IF (@newPassWord = NULL OR @newPassWord = '')
	BEGIN
		UPDATE dbo.Staff SET fullName = @fullName, phoneNumber = @phoneNumber WHERE idStaff = @idStaff
	END
	ELSE
	BEGIN
		UPDATE dbo.Staff SET fullName = @fullName, phoneNumber = @phoneNumber WHERE idStaff = @idStaff
		UPDATE dbo.Account SET passWord = @newPassWord WHERE idStaff = @idStaff AND userName = @userName
	END
END
GO

--thong ke proc
GO
CREATE PROC USP_GetListBillByDates
@checkIn date, @checkOut date
AS
BEGIN
	SELECT b.idBill AS [Mã bill], b.idStaff AS [Mã nhân viên] ,t.nameTable AS [Tên bàn], b.total AS [Tổng tiền], DateCheckIn AS [Ngày vào], DateCheckOut AS [Ngày ra], discount AS [Giảm giá (%)]
	FROM dbo.Bill AS b, dbo.TableFood AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1
	AND t.idTable = b.idTable
END
GO

-- Tao tai khoan connect sql cho client chi co quyen select 
--CREATE LOGIN client WITH PASSWORD = '1234';
--USE QLNH1;
--CREATE USER client FOR LOGIN client;
--GRANT SELECT ON TableFood TO client;
--GRANT SELECT ON Staff TO client;
--GRANT SELECT ON Account TO client;
--GRANT SELECT ON FoodCategory TO client;
--GRANT SELECT ON Food TO client;
--GRANT SELECT ON Bill TO client;
--GRANT SELECT ON BillInfo TO client;