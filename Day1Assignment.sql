CREATE DATABASE ECOMMERCE_ASSIGNMENT_DB;


USE ECOMMERCE_ASSIGNMENT_DB;


CREATE TABLE Customer
(
    CustomerId INT PRIMARY KEY,
    CustomerName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    MobileNo VARCHAR(15),
    City VARCHAR(50),
    Address VARCHAR(200),
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE Seller
(
    SellerId INT PRIMARY KEY,
    SellerName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    MobileNo VARCHAR(15),
    City VARCHAR(50),
    Rating DECIMAL(3,2),
    IsActive BIT DEFAULT 1
);

CREATE TABLE Product
(
    ProductId INT PRIMARY KEY,
    ProductName VARCHAR(100) NOT NULL,
    Category VARCHAR(50),
    Price DECIMAL(10,2) CHECK (Price > 0),
    StockQuantity INT CHECK (StockQuantity >= 0),
    SellerId INT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (SellerId) REFERENCES Seller(SellerId)
);

CREATE TABLE Orders
(
    OrderId INT PRIMARY KEY,
    CustomerId INT,
    OrderDate DATETIME DEFAULT GETDATE(),
    OrderStatus VARCHAR(50) DEFAULT 'Pending',
    PaymentMode VARCHAR(50),
    DeliveryCity VARCHAR(50),
    FOREIGN KEY(CustomerId) REFERENCES Customer(CustomerId)
);

CREATE TABLE OrderItem
(
    OrderItemId INT PRIMARY KEY,
    OrderId INT,
    ProductId INT,
    Quantity INT CHECK(Quantity > 0),
    UnitPrice DECIMAL(10,2),
    FOREIGN KEY(OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY(ProductId) REFERENCES Product(ProductId)
);

INSERT INTO Customer VALUES(1,'Charan','Charan@gmail.com','9876543210','Chennai','Anna Nagar',1,GETDATE()),(2,'Raju','Raju@gmail.com','9876543211','Bangalore','MG Road',1,GETDATE()), (3,'Arjun','Arjun@gmail.com','9876543212','Hyderabad','Madhapur',1,GETDATE()),(4,'Rao','Rao@gmail.com','9876543213','Chennai','Velachery',1,GETDATE()), (5,'Pawan','Pawan@gmail.com','9876543214','Mumbai','Andheri',1,GETDATE());

INSERT INTO Seller VALUES (1,'Amazon Seller','amazon@gmail.com','9000000001','Chennai',4.5,1),(2,'Flipkart Seller','flipkart@gmail.com','9000000002','Bangalore',4.3,1),(3,'Reliance Seller','reliance@gmail.com','9000000003','Mumbai',4.7,1), (4,'Croma Seller','croma@gmail.com','9000000004','Hyderabad',4.4,1);

INSERT INTO Product VALUES(1,'iPhone 15','Mobile',75000,20,1,GETDATE()),(2,'Samsung S24','Mobile',65000,15,1,GETDATE()),(3,'Dell Inspiron','Laptop',55000,10,2,GETDATE()),(4,'HP Pavilion','Laptop',60000,8,2,GETDATE()),(5,'Sony TV','Electronics',45000,12,3,GETDATE()), (6,'LG TV','Electronics',40000,7,3,GETDATE()),(7,'Boat Earbuds','Accessories',3000,50,4,GETDATE()),(8,'OnePlus Phone','Mobile',35000,18,4,GETDATE());

INSERT INTO Orders
VALUES(101,1,GETDATE(),'Delivered','UPI','Chennai'),(102,2,GETDATE(),'Pending','Card','Bangalore'), (103,3,GETDATE(),'Shipped','UPI','Hyderabad'),(104,1,GETDATE(),'Delivered','Cash','Chennai'),(105,5,GETDATE(),'Pending','Card','Mumbai');

INSERT INTO OrderItem
VALUES(1,101,1,1,75000),(2,101,7,2,3000),(3,102,3,1,55000),(4,102,5,1,45000),(5,103,2,1,65000),(6,103,7,3,3000),(7,104,4,1,60000),(8,104,6,1,40000),(9,105,8,1,35000),(10,105,7,2,3000);

UPDATE Customer SET City='Delhi' WHERE CustomerId=1;

UPDATE Product SET Price=80000 WHERE ProductId=1;

UPDATE Orders SET OrderStatus='Delivered' WHERE OrderId=102;

INSERT INTO Product VALUES (9, 'Test Headphones', 'Accessories', 2000, 5, 4, GETDATE());
DELETE FROM Product WHERE ProductId = 9;

SELECT * FROM Customer;
SELECT * FROM Seller;
SELECT * FROM Product;
SELECT * FROM Orders;
SELECT * FROM OrderItem;

SELECT * FROM Customer WHERE City='Chennai';

SELECT * FROM Customer WHERE City<>'Chennai';

SELECT * FROM Product WHERE Price>50000;

SELECT * FROM Product WHERE Price BETWEEN 10000 AND 60000;

SELECT * FROM Product WHERE Category IN ('Mobile','Laptop');

SELECT * FROM Customer WHERE CustomerName LIKE 'A%';

SELECT * FROM Customer WHERE Email LIKE '%gmail%';

SELECT * FROM Product WHERE ProductName LIKE '%Phone%';

SELECT * FROM Orders WHERE OrderStatus='Delivered';

SELECT * FROM Product WHERE StockQuantity<10;

SELECT * FROM Customer WHERE MobileNo IS NOT NULL;

SELECT * FROM Product WHERE Price NOT BETWEEN 10000 AND 50000;

SELECT * FROM Customer WHERE City IN ('Chennai','Bangalore');

SELECT * FROM Customer WHERE City='Chennai' AND IsActive=1;

SELECT * FROM Customer WHERE City<>'Hyderabad';

SELECT City, COUNT(*) AS TotalCustomers FROM Customer GROUP BY City;

SELECT Category, COUNT(*) AS TotalProducts FROM Product GROUP BY Category;

SELECT Category, SUM(StockQuantity) AS TotalStock FROM Product GROUP BY Category;

SELECT Category, MAX(Price) AS MaxPrice FROM Product GROUP BY Category;

SELECT Category, MIN(Price) AS MinPrice
FROM Product
GROUP BY Category;

SELECT Category, AVG(Price) AS AvgPrice FROM Product GROUP BY Category;

SELECT C.CustomerName, SUM(OI.Quantity * OI.UnitPrice) AS TotalAmount
FROM Customer C
JOIN Orders O ON C.CustomerId = O.CustomerId
JOIN OrderItem OI ON O.OrderId = OI.OrderId
GROUP BY C.CustomerName;

SELECT P.ProductName,
SUM(OI.Quantity * OI.UnitPrice) AS TotalSales
FROM Product P
JOIN OrderItem OI ON P.ProductId = OI.ProductId
GROUP BY P.ProductName;

SELECT P.ProductName,
SUM(OI.Quantity) AS TotalQty
FROM Product P
JOIN OrderItem OI ON P.ProductId = OI.ProductId
GROUP BY P.ProductName;

SELECT Category, COUNT(*) AS TotalProducts
FROM Product
GROUP BY Category
HAVING COUNT(*) > 1;

SELECT C.CustomerName,
SUM(OI.Quantity * OI.UnitPrice) AS TotalAmount
FROM Customer C
JOIN Orders O ON C.CustomerId = O.CustomerId
JOIN OrderItem OI ON O.OrderId = OI.OrderId
GROUP BY C.CustomerName
HAVING SUM(OI.Quantity * OI.UnitPrice) > 50000;

SELECT S.SellerName,
COUNT(P.ProductId) AS ProductCount
FROM Seller S
JOIN Product P ON S.SellerId = P.SellerId
GROUP BY S.SellerName;

SELECT S.SellerName,
SUM(OI.Quantity * OI.UnitPrice) AS TotalSales
FROM Seller S
JOIN Product P ON S.SellerId = P.SellerId
JOIN OrderItem OI ON P.ProductId = OI.ProductId
GROUP BY S.SellerName;

SELECT OrderStatus,
COUNT(*) AS OrderCount
FROM Orders
GROUP BY OrderStatus;

SELECT City,
COUNT(*) AS TotalCustomers
FROM Customer
GROUP BY City
ORDER BY TotalCustomers DESC;

SELECT * FROM Product ORDER BY Price ASC;

SELECT * FROM Product ORDER BY Price DESC;

SELECT * FROM Customer ORDER BY City ASC, CustomerName ASC;

SELECT * FROM Orders ORDER BY OrderDate DESC;

SELECT * FROM Product ORDER BY Category ASC, Price DESC;

SELECT TOP 3 * FROM Product ORDER BY Price DESC;

SELECT TOP 5 * FROM Orders ORDER BY OrderDate DESC;

SELECT * FROM Customer ORDER BY IsActive DESC, CustomerName ASC;

SELECT * FROM Orders O
INNER JOIN Customer C
ON O.CustomerId = C.CustomerId;

SELECT * FROM Product P
INNER JOIN Seller S
ON P.SellerId = S.SellerId;

SELECT *FROM OrderItem OI
INNER JOIN Product P
ON OI.ProductId = P.ProductId;

SELECT
C.CustomerName,
O.OrderId,
P.ProductName,
S.SellerName,
OI.Quantity,
OI.UnitPrice
FROM Customer C
JOIN Orders O ON C.CustomerId = O.CustomerId
JOIN OrderItem OI ON O.OrderId = OI.OrderId
JOIN Product P ON OI.ProductId = P.ProductId
JOIN Seller S ON P.SellerId = S.SellerId;

SELECT *FROM Customer C
LEFT JOIN Orders O
ON C.CustomerId = O.CustomerId;

SELECT *  FROM Customer C
RIGHT JOIN Orders O
ON C.CustomerId = O.CustomerId;

SELECT * FROM Customer C
FULL OUTER JOIN Orders O
ON C.CustomerId = O.CustomerId;

SELECT * FROM Customer
CROSS JOIN Product;

SELECT * FROM Customer C
LEFT JOIN Orders O
ON C.CustomerId = O.CustomerId
WHERE O.OrderId IS NULL;

SELECT * FROM Product P
LEFT JOIN OrderItem OI
ON P.ProductId = OI.ProductId
WHERE OI.ProductId IS NULL;

SELECT S.SellerName, P.ProductName FROM Seller S
JOIN Product P
ON S.SellerId = P.SellerId;

SELECT C.CustomerName ,P.ProductName FROM Customer C
JOIN Orders O ON C.CustomerId = O.CustomerId
JOIN OrderItem OI ON O.OrderId = OI.OrderId
JOIN Product P ON OI.ProductId = P.ProductId;

SELECT O.OrderId, SUM(OI.Quantity * OI.UnitPrice) AS TotalAmount FROM Orders O
JOIN OrderItem OI
ON O.OrderId = OI.OrderId
GROUP BY O.OrderId;

SELECT S.SellerName, SUM(OI.Quantity * OI.UnitPrice) AS TotalSales FROM Seller S
JOIN Product P ON S.SellerId = P.SellerId
JOIN OrderItem OI ON P.ProductId = OI.ProductId
GROUP BY S.SellerName;

SELECT P.ProductName, SUM(OI.Quantity) AS TotalQuantitySold FROM Product P
JOIN OrderItem OI
ON P.ProductId = OI.ProductId
GROUP BY P.ProductName;

--Stored Procedures
CREATE PROC CustomerOrders
AS
BEGIN
	SELECT
		C.CustomerId,
		C.CustomerName,
		O.OrderId,
		O.OrderDate,
		O.OrderStatus
	FROM Customer C
	JOIN Orders O
		ON C.CustomerId = O.CustomerId
END


EXEC CustomerOrders


CREATE PROC SellerProducts
AS
BEGIN
	SELECT
		S.SellerId,
		S.SellerName,
		P.ProductId,
		P.ProductName,
		P.Price
	FROM Seller S
	JOIN Product P
		ON S.SellerId = P.SellerId
END


EXEC SellerProducts


CREATE PROC OrderItemDetails
AS
BEGIN
	SELECT
		OI.OrderItemId,
		P.ProductName,
		OI.Quantity,
		OI.UnitPrice
	FROM OrderItem OI
	JOIN Product P
		ON OI.ProductId = P.ProductId
END


EXEC OrderItemDetails


CREATE PROC CustomerPurchaseSummary
AS
BEGIN
	SELECT
		C.CustomerName,
		SUM(OI.Quantity * OI.UnitPrice) AS TotalPurchase
	FROM Customer C
	JOIN Orders O
		ON C.CustomerId = O.CustomerId
	JOIN OrderItem OI
		ON O.OrderId = OI.OrderId
	GROUP BY C.CustomerName
END


EXEC CustomerPurchaseSummary


CREATE PROC SellerSalesSummary
AS
BEGIN
	SELECT
		S.SellerName,
		SUM(OI.Quantity * OI.UnitPrice) AS TotalSales
	FROM Seller S
	JOIN Product P
		ON S.SellerId = P.SellerId
	JOIN OrderItem OI
		ON P.ProductId = OI.ProductId
	GROUP BY S.SellerName
END


EXEC SellerSalesSummary


CREATE PROC CustomersByCity
	@City VARCHAR(50)
AS
BEGIN
	SELECT *
	FROM Customer
	WHERE City = @City
END


EXEC CustomersByCity 'Chennai'


CREATE PROC ProductsByCategory
	@Category VARCHAR(50)
AS
BEGIN
	SELECT *
	FROM Product
	WHERE Category = @Category
END


EXEC ProductsByCategory 'Mobile'


CREATE PROC TotalCustomers
	@Total INT OUTPUT
AS
BEGIN
	SELECT @Total = COUNT(*)
	FROM Customer
END


DECLARE @CustomerCount INT

EXEC TotalCustomers @CustomerCount OUTPUT

SELECT @CustomerCount AS TotalCustomers


    
CREATE PROC TotalProducts
	@Total INT OUTPUT
AS
BEGIN
	SELECT @Total = COUNT(*)
	FROM Product
END


DECLARE @ProductCount INT

EXEC TotalProducts @ProductCount OUTPUT

SELECT @ProductCount AS TotalProducts


CREATE PROC TotalOrders
	@Total INT OUTPUT
AS
BEGIN
	SELECT @Total = COUNT(*)
	FROM Orders
END


DECLARE @OrderCount INT

EXEC TotalOrders @OrderCount OUTPUT

SELECT @OrderCount AS TotalOrders
