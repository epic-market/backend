INSERT INTO Catalogs (BusinessID, Barcode, Name, Description, Images, Category, Rate, IsActive, InStock, IsRecommended, MaximumOrderPurchase, Rating, ReviewCount, OrderCount, Status, CreateDate)
VALUES 
    (6, 1234567890, 'Product 1', 'Description of Product 1', 'image1.jpg', 'Category 1', 10.99, 1, 1, 1, 5, 4.5, 10, 20, 'Active', GETDATE()),
    (6, 1234567891, 'Product 2', 'Description of Product 2', 'image2.jpg', 'Category 2', 15.99, 1, 1, 0, NULL, NULL, NULL, NULL, 'Inactive', GETDATE()),
    (6, 1234567892, 'Product 3', 'Description of Product 3', 'image3.jpg', 'Category 1', 20.99, 1, 1, 1, 5, 4.2, 8, 15, 'Active', GETDATE()),
    (6, 1234567893, 'Product 4', 'Description of Product 4', 'image4.jpg', 'Category 2', 25.99, 1, 0, 1, 10, 4.8, 15, 25, 'Active', GETDATE()),
    (6, 1234567894, 'Product 5', 'Description of Product 5', 'image5.jpg', 'Category 3', 30.99, 1, 1, 1, 5, 4.6, 12, 30, 'Active', GETDATE()),
    (6, 1234567895, 'Product 6', 'Description of Product 6', 'image6.jpg', 'Category 1', 35.99, 1, 1, 0, NULL, NULL, NULL, NULL, 'Inactive', GETDATE()),
    (6, 1234567896, 'Product 7', 'Description of Product 7', 'image7.jpg', 'Category 2', 40.99, 1, 1, 1, 8, 4.3, 20, 40, 'Active', GETDATE()),
    (6, 1234567897, 'Product 8', 'Description of Product 8', 'image8.jpg', 'Category 3', 45.99, 1, 0, 1, 12, 4.7, 18, 35, 'Active', GETDATE()),
    (6, 1234567898, 'Product 9', 'Description of Product 9', 'image9.jpg', 'Category 1', 50.99, 1, 1, 1, 6, 4.9, 22, 45, 'Active', GETDATE()),
    (6, 1234567899, 'Product 10', 'Description of Product 10', 'image10.jpg', 'Category 2', 55.99, 1, 1, 0, NULL, NULL, NULL, NULL, 'Inactive', GETDATE());


INSERT INTO OutletProducts (OutletId, ProductId)
VALUES 
    (1, 3),
    (1, 4),
    (1, 5);