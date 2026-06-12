USE ECOMMERCE_ASSIGNMENT_DB


SELECT * FROM Product
WHERE Price > (SELECT AVG(Price) FROM Product)

SELECT * FROM Product
WHERE StockQuantity < (SELECT AVG(StockQuantity) FROM Product)

SELECT * FROM Customer
WHERE CustomerId IN (SELECT CustomerId FROM Orders)

SELECT * FROM Customer
WHERE CustomerId NOT IN (SELECT CustomerId FROM Orders)

SELECT * FROM Product
WHERE ProductId IN (SELECT ProductId FROM OrderItem)

SELECT * FROM Product
WHERE ProductId NOT IN (SELECT ProductId FROM OrderItem)

SELECT * FROM Seller
WHERE SellerId IN (SELECT SellerId FROM Product)

SELECT * FROM Seller
WHERE SellerId NOT IN (SELECT SellerId FROM Product)

SELECT * FROM Orders
WHERE CustomerId IN
(
    SELECT CustomerId FROM Customer
    WHERE City = 'Delhi'
)

SELECT * FROM Product
WHERE SellerId IN
(
    SELECT SellerId FROM Seller
    WHERE City = 'Chennai'
)

SELECT * FROM Orders
WHERE OrderId IN
(
    SELECT OrderId FROM OrderItem
    WHERE ProductId IN
    (
        SELECT ProductId FROM Product
        WHERE Category = 'Mobile'
    )
)

SELECT * FROM Orders
WHERE OrderId NOT IN
(
    SELECT OrderId FROM OrderItem
    WHERE ProductId IN
    (
        SELECT ProductId FROM Product
        WHERE Category = 'Laptop'
    )
)

SELECT * FROM Product WHERE Price = (SELECT MAX(Price) FROM Product)
SELECT * FROM Product WHERE Price = (SELECT MIN(Price) FROM Product)
SELECT * FROM Product WHERE Price > (SELECT AVG(Price) FROM Product)
SELECT * FROM Product WHERE Price < (SELECT AVG(Price) FROM Product)

SELECT * FROM Customer
WHERE CustomerId IN
(
    SELECT O.CustomerId
    FROM Orders O
    JOIN OrderItem OI ON O.OrderId = OI.OrderId
    JOIN Product P ON OI.ProductId = P.ProductId
    GROUP BY O.CustomerId
    HAVING SUM(OI.Quantity * P.Price) >
    (
        SELECT AVG(OrderAmount) FROM
        (
            SELECT OI2.OrderId, SUM(OI2.Quantity * P2.Price) AS OrderAmount
            FROM OrderItem OI2
            JOIN Product P2 ON OI2.ProductId = P2.ProductId
            GROUP BY OI2.OrderId
        ) AS T
    )
)

SELECT * FROM Seller
WHERE SellerId IN
(
    SELECT P.SellerId FROM Product P
    JOIN OrderItem OI ON P.ProductId = OI.ProductId
    GROUP BY P.SellerId
    HAVING SUM(OI.Quantity * P.Price) > 50000
)

SELECT * FROM Product
WHERE ProductId IN
(
    SELECT ProductId FROM OrderItem
    GROUP BY ProductId
    HAVING SUM(Quantity) >
    (
        SELECT AVG(TotalQty) FROM
        (
            SELECT SUM(Quantity) AS TotalQty
            FROM OrderItem
            GROUP BY ProductId
        ) AS T
    )
)

SELECT * FROM Customer
WHERE CustomerId IN
(
    SELECT O.CustomerId
    FROM Orders O
    JOIN OrderItem OI ON O.OrderId = OI.OrderId
    JOIN Product P ON OI.ProductId = P.ProductId
    GROUP BY O.CustomerId
    HAVING SUM(OI.Quantity * P.Price) =
    (
        SELECT MAX(TotalSpent) FROM
        (
            SELECT O2.CustomerId, SUM(OI2.Quantity * P2.Price) AS TotalSpent
            FROM Orders O2
            JOIN OrderItem OI2 ON O2.OrderId = OI2.OrderId
            JOIN Product P2 ON OI2.ProductId = P2.ProductId
            GROUP BY O2.CustomerId
        ) AS T
    )
)

SELECT * FROM Product
WHERE ProductId IN
(
    SELECT OI.ProductId
    FROM OrderItem OI
    JOIN Product P ON OI.ProductId = P.ProductId
    GROUP BY OI.ProductId
    HAVING SUM(OI.Quantity * P.Price) =
    (
        SELECT MAX(SalesAmount) FROM
        (
            SELECT OI2.ProductId, SUM(OI2.Quantity * P2.Price) AS SalesAmount
            FROM OrderItem OI2
            JOIN Product P2 ON OI2.ProductId = P2.ProductId
            GROUP BY OI2.ProductId
        ) AS T
    )
)

SELECT * FROM Seller
WHERE SellerId IN
(
    SELECT P.SellerId FROM Product P
    JOIN OrderItem OI ON P.ProductId = OI.ProductId
    GROUP BY P.SellerId
    HAVING SUM(OI.Quantity * P.Price) =
    (
        SELECT MAX(TotalSales) FROM
        (
            SELECT P2.SellerId, SUM(OI2.Quantity * P2.Price) AS TotalSales
            FROM Product P2
            JOIN OrderItem OI2 ON P2.ProductId = OI2.ProductId
            GROUP BY P2.SellerId
        ) AS T
    )
)

SELECT * FROM Product P1
WHERE P1.Price > (SELECT AVG(P2.Price) FROM Product P2 WHERE P2.Category = P1.Category)

SELECT * FROM Product P1
WHERE P1.Price < (SELECT AVG(P2.Price) FROM Product P2 WHERE P2.Category = P1.Category)

SELECT * FROM Seller S
WHERE (SELECT COUNT(*) FROM Product P WHERE P.SellerId = S.SellerId) > 2

SELECT * FROM Customer C
WHERE (SELECT COUNT(*) FROM Orders O WHERE O.CustomerId = C.CustomerId) > 1

SELECT * FROM Orders O
WHERE
(
    SELECT SUM(OI.Quantity * P.Price)
    FROM OrderItem OI
    JOIN Product P ON OI.ProductId = P.ProductId
    WHERE OI.OrderId = O.OrderId
) >
(
    SELECT AVG(OrderAmount) FROM
    (
        SELECT OI2.OrderId, SUM(OI2.Quantity * P2.Price) AS OrderAmount
        FROM OrderItem OI2
        JOIN Product P2 ON OI2.ProductId = P2.ProductId
        GROUP BY OI2.OrderId
    ) AS T
)

SELECT * FROM Product P1
WHERE P1.StockQuantity > (SELECT AVG(P2.StockQuantity) FROM Product P2 WHERE P2.Category = P1.Category)

SELECT * FROM Seller S
WHERE (SELECT AVG(P.Price) FROM Product P WHERE P.SellerId = S.SellerId) > (SELECT AVG(Price) FROM Product)

SELECT * FROM Customer C
WHERE EXISTS (SELECT 1 FROM Orders O WHERE O.CustomerId = C.CustomerId)

SELECT * FROM Customer C
WHERE NOT EXISTS (SELECT 1 FROM Orders O WHERE O.CustomerId = C.CustomerId)

SELECT * FROM Product P
WHERE EXISTS (SELECT 1 FROM OrderItem OI WHERE OI.ProductId = P.ProductId)

SELECT * FROM Product P
WHERE NOT EXISTS (SELECT 1 FROM OrderItem OI WHERE OI.ProductId = P.ProductId)

SELECT * FROM Seller S
WHERE EXISTS (SELECT 1 FROM Product P WHERE P.SellerId = S.SellerId)

SELECT * FROM Seller S
WHERE NOT EXISTS (SELECT 1 FROM Product P WHERE P.SellerId = S.SellerId)

SELECT * FROM Customer C
WHERE EXISTS
(
    SELECT 1 FROM Orders O
    JOIN OrderItem OI ON O.OrderId = OI.OrderId
    JOIN Product P ON OI.ProductId = P.ProductId
    WHERE O.CustomerId = C.CustomerId AND P.Category = 'Mobile'
)

SELECT * FROM Customer C
WHERE NOT EXISTS
(
    SELECT 1 FROM Orders O
    JOIN OrderItem OI ON O.OrderId = OI.OrderId
    JOIN Product P ON OI.ProductId = P.ProductId
    WHERE O.CustomerId = C.CustomerId AND P.Category = 'Laptop'
)

CREATE OR ALTER PROCEDURE usp_GetAllCustomers AS
BEGIN SELECT * FROM Customer END


CREATE OR ALTER PROCEDURE usp_GetAllProducts AS
BEGIN SELECT * FROM Product END


CREATE OR ALTER PROCEDURE usp_GetAllSellers AS
BEGIN SELECT * FROM Seller END


CREATE OR ALTER PROCEDURE usp_GetAllOrders AS
BEGIN SELECT * FROM Orders END


CREATE OR ALTER PROCEDURE usp_GetAllOrderItems AS
BEGIN SELECT * FROM OrderItem END


CREATE OR ALTER PROCEDURE usp_GetCustomerById @CustomerId INT AS
BEGIN SELECT * FROM Customer WHERE CustomerId = @CustomerId END


CREATE OR ALTER PROCEDURE usp_GetProductById @ProductId INT AS
BEGIN SELECT * FROM Product WHERE ProductId = @ProductId END


CREATE OR ALTER PROCEDURE usp_GetSellerById @SellerId INT AS
BEGIN SELECT * FROM Seller WHERE SellerId = @SellerId END


CREATE OR ALTER PROCEDURE usp_GetOrderById @OrderId INT AS
BEGIN SELECT * FROM Orders WHERE OrderId = @OrderId END


CREATE OR ALTER PROCEDURE usp_GetCustomersByCity @City VARCHAR(100) AS
BEGIN SELECT * FROM Customer WHERE City = @City END


CREATE OR ALTER PROCEDURE usp_GetProductsByCategory @Category VARCHAR(100) AS
BEGIN SELECT * FROM Product WHERE Category = @Category END


CREATE OR ALTER PROCEDURE usp_GetProductsBySellerId @SellerId INT AS
BEGIN SELECT * FROM Product WHERE SellerId = @SellerId END


CREATE OR ALTER PROCEDURE usp_GetOrdersByCustomerId @CustomerId INT AS
BEGIN SELECT * FROM Orders WHERE CustomerId = @CustomerId END


CREATE OR ALTER PROCEDURE usp_GetOrderItemsByOrderId @OrderId INT AS
BEGIN SELECT * FROM OrderItem WHERE OrderId = @OrderId END


CREATE OR ALTER PROCEDURE usp_GetProductsGreaterThanPrice @Price MONEY AS
BEGIN SELECT * FROM Product WHERE Price > @Price END


CREATE OR ALTER PROCEDURE usp_InsertCustomer
@CustomerName VARCHAR(100), @Email VARCHAR(100), @MobileNo BIGINT,
@City VARCHAR(100), @Address VARCHAR(200), @IsActive BIT
AS
BEGIN
    INSERT INTO Customer(CustomerName, Email, MobileNo, City, Address, IsActive, CreatedDate)
    VALUES(@CustomerName, @Email, @MobileNo, @City, @Address, @IsActive, GETDATE())
END


CREATE OR ALTER PROCEDURE usp_InsertSeller
@SellerName VARCHAR(100), @Email VARCHAR(100), @MobileNo BIGINT,
@City VARCHAR(100), @Rating DECIMAL(3,1), @IsActive BIT
AS
BEGIN
    INSERT INTO Seller(SellerName, Email, MobileNo, City, Rating, IsActive)
    VALUES(@SellerName, @Email, @MobileNo, @City, @Rating, @IsActive)
END


CREATE OR ALTER PROCEDURE usp_InsertProduct
@ProductName VARCHAR(100), @Category VARCHAR(100),
@Price MONEY, @StockQuantity INT, @SellerId INT
AS
BEGIN
    INSERT INTO Product(ProductName, Category, Price, StockQuantity, SellerId, CreatedDate)
    VALUES(@ProductName, @Category, @Price, @StockQuantity, @SellerId, GETDATE())
END


CREATE OR ALTER PROCEDURE usp_InsertOrder
@CustomerId INT, @OrderStatus VARCHAR(50), @PaymentMode VARCHAR(50), @DeliveryCity VARCHAR(100)
AS
BEGIN
    INSERT INTO Orders(CustomerId, OrderDate, OrderStatus, PaymentMode, DeliveryCity)
    VALUES(@CustomerId, GETDATE(), @OrderStatus, @PaymentMode, @DeliveryCity)
END


CREATE OR ALTER PROCEDURE usp_InsertOrderItem @OrderId INT, @ProductId INT, @Quantity INT AS
BEGIN
    INSERT INTO OrderItem(OrderId, ProductId, Quantity) VALUES(@OrderId, @ProductId, @Quantity)
END


CREATE OR ALTER PROCEDURE usp_UpdateCustomerCity @CustomerId INT, @City VARCHAR(100) AS
BEGIN UPDATE Customer SET City = @City WHERE CustomerId = @CustomerId END


CREATE OR ALTER PROCEDURE usp_UpdateCustomerMobile @CustomerId INT, @MobileNo BIGINT AS
BEGIN UPDATE Customer SET MobileNo = @MobileNo WHERE CustomerId = @CustomerId END


CREATE OR ALTER PROCEDURE usp_UpdateProductPrice @ProductId INT, @Price MONEY AS
BEGIN UPDATE Product SET Price = @Price WHERE ProductId = @ProductId END


CREATE OR ALTER PROCEDURE usp_UpdateProductStock @ProductId INT, @StockQuantity INT AS
BEGIN UPDATE Product SET StockQuantity = @StockQuantity WHERE ProductId = @ProductId END


CREATE OR ALTER PROCEDURE usp_UpdateOrderStatus @OrderId INT, @OrderStatus VARCHAR(50) AS
BEGIN UPDATE Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId END


CREATE OR ALTER PROCEDURE usp_UpdateSellerRating @SellerId INT, @Rating DECIMAL(3,1) AS
BEGIN UPDATE Seller SET Rating = @Rating WHERE SellerId = @SellerId END


CREATE OR ALTER PROCEDURE usp_UpdateCustomerActiveStatus @CustomerId INT, @IsActive BIT AS
BEGIN UPDATE Customer SET IsActive = @IsActive WHERE CustomerId = @CustomerId END


CREATE OR ALTER PROCEDURE usp_UpdateSellerActiveStatus @SellerId INT, @IsActive BIT AS
BEGIN UPDATE Seller SET IsActive = @IsActive WHERE SellerId = @SellerId END


CREATE OR ALTER PROCEDURE usp_DeleteCustomer @CustomerId INT AS
BEGIN DELETE FROM Customer WHERE CustomerId = @CustomerId END


CREATE OR ALTER PROCEDURE usp_DeleteSeller @SellerId INT AS
BEGIN DELETE FROM Seller WHERE SellerId = @SellerId END


CREATE OR ALTER PROCEDURE usp_DeleteProduct @ProductId INT AS
BEGIN DELETE FROM Product WHERE ProductId = @ProductId END


CREATE OR ALTER PROCEDURE usp_DeleteOrder @OrderId INT AS
BEGIN
    DELETE FROM OrderItem WHERE OrderId = @OrderId
    DELETE FROM Orders WHERE OrderId = @OrderId
END


CREATE OR ALTER PROCEDURE usp_DeleteOrderItem @OrderItemId INT AS
BEGIN DELETE FROM OrderItem WHERE OrderItemId = @OrderItemId END


CREATE OR ALTER PROCEDURE usp_CustomerWiseOrderDetails AS
BEGIN
    SELECT C.CustomerId, C.CustomerName, O.OrderId, O.OrderDate,
           O.OrderStatus, O.PaymentMode, O.DeliveryCity
    FROM Customer C JOIN Orders O ON C.CustomerId = O.CustomerId
END


CREATE OR ALTER PROCEDURE usp_SellerWiseProductDetails AS
BEGIN
    SELECT S.SellerId, S.SellerName, P.ProductId, P.ProductName,
           P.Category, P.Price, P.StockQuantity
    FROM Seller S JOIN Product P ON S.SellerId = P.SellerId
END


CREATE OR ALTER PROCEDURE usp_OrderWiseProductDetails AS
BEGIN
    SELECT O.OrderId, P.ProductId, P.ProductName,
           OI.Quantity, P.Price, OI.Quantity * P.Price AS TotalAmount
    FROM Orders O
    JOIN OrderItem OI ON O.OrderId = OI.OrderId
    JOIN Product P ON OI.ProductId = P.ProductId
END


CREATE OR ALTER PROCEDURE usp_CompleteOrderReport AS
BEGIN
    SELECT C.CustomerName, O.OrderId, O.OrderDate, O.OrderStatus,
           O.PaymentMode, O.DeliveryCity, P.ProductName, S.SellerName,
           OI.Quantity, P.Price, OI.Quantity * P.Price AS TotalAmount
    FROM Customer C
    JOIN Orders O ON C.CustomerId = O.CustomerId
    JOIN OrderItem OI ON O.OrderId = OI.OrderId
    JOIN Product P ON OI.ProductId = P.ProductId
    JOIN Seller S ON P.SellerId = S.SellerId
END


CREATE OR ALTER PROCEDURE usp_CustomerWiseTotalOrderAmount AS
BEGIN
    SELECT C.CustomerId, C.CustomerName,
           SUM(OI.Quantity * P.Price) AS TotalOrderAmount
    FROM Customer C
    JOIN Orders O ON C.CustomerId = O.CustomerId
    JOIN OrderItem OI ON O.OrderId = OI.OrderId
    JOIN Product P ON OI.ProductId = P.ProductId
    GROUP BY C.CustomerId, C.CustomerName
END


CREATE OR ALTER PROCEDURE usp_SellerWiseTotalSalesAmount AS
BEGIN
    SELECT S.SellerId, S.SellerName,
           SUM(OI.Quantity * P.Price) AS TotalSalesAmount
    FROM Seller S
    JOIN Product P ON S.SellerId = P.SellerId
    JOIN OrderItem OI ON P.ProductId = OI.ProductId
    GROUP BY S.SellerId, S.SellerName
END


CREATE OR ALTER PROCEDURE usp_ProductWiseTotalSalesQuantity AS
BEGIN
    SELECT P.ProductId, P.ProductName, SUM(OI.Quantity) AS TotalSalesQuantity
    FROM Product P JOIN OrderItem OI ON P.ProductId = OI.ProductId
    GROUP BY P.ProductId, P.ProductName
END


CREATE OR ALTER PROCEDURE usp_GetTotalCustomers @TotalCustomers INT OUTPUT AS
BEGIN SELECT @TotalCustomers = COUNT(*) FROM Customer END


CREATE OR ALTER PROCEDURE usp_GetTotalProducts @TotalProducts INT OUTPUT AS
BEGIN SELECT @TotalProducts = COUNT(*) FROM Product END


CREATE OR ALTER PROCEDURE usp_GetTotalOrders @TotalOrders INT OUTPUT AS
BEGIN SELECT @TotalOrders = COUNT(*) FROM Orders END


CREATE OR ALTER PROCEDURE usp_GetTotalSalesAmountOfProduct
@ProductId INT, @TotalSalesAmount MONEY OUTPUT AS
BEGIN
    SELECT @TotalSalesAmount = ISNULL(SUM(OI.Quantity * P.Price), 0)
    FROM OrderItem OI JOIN Product P ON OI.ProductId = P.ProductId
    WHERE OI.ProductId = @ProductId
END


CREATE OR ALTER PROCEDURE usp_GetTotalPurchaseAmountOfCustomer
@CustomerId INT, @TotalPurchaseAmount MONEY OUTPUT AS
BEGIN
    SELECT @TotalPurchaseAmount = ISNULL(SUM(OI.Quantity * P.Price), 0)
    FROM Orders O
    JOIN OrderItem OI ON O.OrderId = OI.OrderId
    JOIN Product P ON OI.ProductId = P.ProductId
    WHERE O.CustomerId = @CustomerId
END


EXEC usp_GetAllCustomers
EXEC usp_GetAllProducts
EXEC usp_GetAllSellers
EXEC usp_GetAllOrders
EXEC usp_GetAllOrderItems

EXEC usp_GetCustomerById 1
EXEC usp_GetProductById 1
EXEC usp_GetSellerById 1
EXEC usp_GetOrderById 101
EXEC usp_GetCustomersByCity 'Delhi'
EXEC usp_GetProductsByCategory 'Mobile'
EXEC usp_GetProductsBySellerId 2
EXEC usp_GetOrdersByCustomerId 1
EXEC usp_GetOrderItemsByOrderId 101
EXEC usp_GetProductsGreaterThanPrice 50000

EXEC usp_InsertCustomer 'Vikram', 'vikram@gmail.com', 9876543215, 'Pune', 'Kothrud', 1
EXEC usp_InsertSeller 'Vijay Sales', 'vijaysales@gmail.com', 9000000005, 'Pune', 4.2, 1
EXEC usp_InsertProduct 'Noise Smartwatch', 'Accessories', 5000, 30, 4
EXEC usp_InsertOrder 3, 'Pending', 'UPI', 'Hyderabad'
EXEC usp_InsertOrderItem 103, 1, 1

EXEC usp_UpdateCustomerCity 2, 'Mysore'
EXEC usp_UpdateCustomerMobile 3, 9876543299
EXEC usp_UpdateProductPrice 2, 67000
EXEC usp_UpdateProductStock 7, 45
EXEC usp_UpdateOrderStatus 103, 'Delivered'
EXEC usp_UpdateSellerRating 3, 4.8
EXEC usp_UpdateCustomerActiveStatus 4, 0
EXEC usp_UpdateSellerActiveStatus 2, 0

EXEC usp_DeleteOrderItem 10
EXEC usp_DeleteOrder 105
EXEC usp_DeleteProduct 8
EXEC usp_DeleteCustomer 5
EXEC usp_DeleteSeller 4

EXEC usp_CustomerWiseOrderDetails
EXEC usp_SellerWiseProductDetails
EXEC usp_OrderWiseProductDetails
EXEC usp_CompleteOrderReport
EXEC usp_CustomerWiseTotalOrderAmount
EXEC usp_SellerWiseTotalSalesAmount
EXEC usp_ProductWiseTotalSalesQuantity

DECLARE @TotalCustomers INT
EXEC usp_GetTotalCustomers @TotalCustomers OUTPUT
SELECT @TotalCustomers AS TotalCustomers

DECLARE @TotalProducts INT
EXEC usp_GetTotalProducts @TotalProducts OUTPUT
SELECT @TotalProducts AS TotalProducts

DECLARE @TotalOrders INT
EXEC usp_GetTotalOrders @TotalOrders OUTPUT
SELECT @TotalOrders AS TotalOrders

DECLARE @SalesAmt MONEY
EXEC usp_GetTotalSalesAmountOfProduct 1, @SalesAmt OUTPUT
SELECT @SalesAmt AS TotalSalesAmountForiPhone15

DECLARE @PurchaseAmt MONEY
EXEC usp_GetTotalPurchaseAmountOfCustomer 1, @PurchaseAmt OUTPUT
SELECT @PurchaseAmt AS TotalPurchaseByCharan
