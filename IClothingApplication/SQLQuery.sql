-- * Wipe Tables
DROP TABLE ItemWrapper;
DROP TABLE ItemDelivery;
DROP TABLE Email;
DROP TABLE OrderStatus;
DROP TABLE ShoppingCart;
DROP TABLE UserComments;
DROP TABLE Product;
DROP TABLE Category;
DROP TABLE Brand;
DROP TABLE Department;
DROP TABLE UserQuery;
DROP TABLE AboutUs;
DROP TABLE UserPassword;
DROP TABLE Administrator;
DROP TABLE Customer;

-- * Manually assign PKs as they are all strings by requirement

-- * User/Admin Password Polymorphism
-- Customer Table
CREATE TABLE Customer (
    customerID INT PRIMARY KEY IDENTITY(1,1),
    customerName VARCHAR(255) NOT NULL,
    customerEmail VARCHAR(255) NOT NULL,
    customerShippingAddress VARCHAR(255) NOT NULL,
    customerBillingAddress VARCHAR(255) NOT NULL,
    customerDOB DATE,
    customerGender VARCHAR(10),
);

-- Administrator Table
CREATE TABLE Administrator (
    -- * Manually assign ID
    adminID INT PRIMARY KEY IDENTITY(1,1),
    adminName VARCHAR(255) NOT NULL,
    adminEmail VARCHAR(255) NOT NULL,
    dateHired DATE NOT NULL
);

-- UserPassword Table
CREATE TABLE UserPassword (
    -- # Assign customerID or adminID
    userAccountName VARCHAR(255) PRIMARY KEY,
    customerID INT,
    adminID INT,
    userEncryptedPassword VARCHAR(255) NOT NULL,
    passwordExpiryTime INT NOT NULL,
    userAccountExpiryDate DATE NOT NULL,
    CHECK ((customerID IS NULL AND adminID IS NOT NULL) OR (customerID IS NOT NULL AND adminID IS NULL)),
    FOREIGN KEY (customerID) REFERENCES Customer(customerID),
    FOREIGN KEY (adminID) REFERENCES Administrator(adminID)
);

-- AboutUs Table
CREATE TABLE AboutUs (
    id INT PRIMARY KEY IDENTITY(1,1),
    companyAddress VARCHAR(255) NOT NULL,
    companyShippingPolicy VARCHAR(255) NOT NULL,
    companyReturnPolicy VARCHAR(255) NOT NULL,
    companyContactInfo VARCHAR(255) NOT NULL,
    companyBusinessDescription VARCHAR(255) NOT NULL,
    managerID INT,
    FOREIGN KEY (managerID) REFERENCES Administrator(adminID)
);

-- UserQuery Table
CREATE TABLE UserQuery (
    queryNo INT PRIMARY KEY IDENTITY(1,1),
    queryDate DATE NOT NULL,
    queryDescription VARCHAR(255) NOT NULL,
    customerID INT NOT NULL,
    FOREIGN KEY (customerID) REFERENCES Customer(customerID)
);

-- * Department/Category Tables
-- Department Table
CREATE TABLE Department (
    departmentID INT PRIMARY KEY IDENTITY(1,1),
    departmentName VARCHAR(255) NOT NULL,
    departmentDescription VARCHAR(255)
);

-- Category Table
CREATE TABLE Category (
    -- # Assign parentCategoryID or parentDepartmentID
    categoryID INT PRIMARY KEY IDENTITY(1,1),
    categoryName VARCHAR(255) NOT NULL,
    categoryDescription VARCHAR(255),
    parentCategoryID INT,
    parentDepartmentID INT,
    CHECK ((parentCategoryID IS NULL AND parentDepartmentID IS NOT NULL) OR (parentCategoryID IS NOT NULL AND parentDepartmentID IS NULL)),
    FOREIGN KEY (parentCategoryID) REFERENCES Category(categoryID),
    FOREIGN KEY (parentDepartmentID) REFERENCES Department(departmentID)
);

-- Brand Table
CREATE TABLE Brand (
    brandID INT PRIMARY KEY IDENTITY(1,1),
    brandName VARCHAR(255) NOT NULL,
    brandDescription VARCHAR(255)
);

-- Product Table
CREATE TABLE Product (
    productID INT PRIMARY KEY IDENTITY(1,1),
    productName VARCHAR(255) NOT NULL,
    productDescription VARCHAR(255),
    productPrice DECIMAL(16, 2) NOT NULL,
    productQty INT NOT NULL,
    categoryID INT NOT NULL,
    brandID INT NOT NULL,
    FOREIGN KEY (categoryID) REFERENCES Category(categoryID),
    FOREIGN KEY (brandID) REFERENCES Brand(brandID)
);

-- UserComments Table
CREATE TABLE UserComments (
    commentNo INT PRIMARY KEY IDENTITY(1,1),
    commentDate DATE NOT NULL,
    commentDescription VARCHAR(255) NOT NULL,
    customerID INT NOT NULL,
    FOREIGN KEY (customerID) REFERENCES Customer(customerID)
);

-- ShoppingCart Table
CREATE TABLE ShoppingCart (
    cartID INT PRIMARY KEY IDENTITY(1,1),
    customerID INT,
    FOREIGN KEY (customerID) REFERENCES Customer(customerID)
);

-- * Wrapper for *-* relationship
CREATE TABLE ItemWrapper (
    productID INT,
    cartID  INT,
    productQty INT NOT NULL,
    PRIMARY KEY (productID, cartID),
    FOREIGN KEY (productID) REFERENCES Product(productID),
    FOREIGN KEY (cartID) REFERENCES ShoppingCart(cartID)
);

-- OrderStatus Table
CREATE TABLE OrderStatus (
    cartID INT PRIMARY KEY,
    status VARCHAR(255) NOT NULL,
    statusDate DATE NOT NULL,
    FOREIGN KEY (cartID) REFERENCES ShoppingCart(cartID)
);

-- Email Table
CREATE TABLE Email (
    emailNo  INT PRIMARY KEY IDENTITY(1,1),
    emailDate DATE NOT NULL,
    emailSubject VARCHAR(255) NOT NULL,
    emailBody TEXT NOT NULL,
    customerID INT NOT NULL,
    adminID INT NOT NULL,
    FOREIGN KEY (customerID) REFERENCES Customer(customerID),
    FOREIGN KEY (adminID) REFERENCES Administrator(adminID)
);

-- ItemDelivery Table
CREATE TABLE ItemDelivery (
    cartID INT PRIMARY KEY,
    stickerID  INT IDENTITY(1,1),
    stickerDate DATE NOT NULL,
    FOREIGN KEY (cartID) REFERENCES ShoppingCart(cartID)
);

-- SQL Queries to create the dummy data

-- Insert Statements for Customer
insert into Customer (customerName, customerEmail, customerShippingAddress, customerBillingAddress, customerDOB, customerGender)
VALUES('Cornell Hoeger', 'Cornell_Hoeger75@hotmail.com', '38404 Deshawn Union', '38404 Deshawn Union', '2023-05-28', 'female'),
      ('Dixie Moen', 'Dixie.Moen62@gmail.com', '163 William Street', '163 William Street', '2006-09-26', 'female'),
      ('Dagmar Windler', 'Dagmar_Windler52@yahoo.com', '4529 Johns Course', '4529 Johns Course', '2000-03-21', 'female'),
      ('Carlee Lakin', 'Carlee.Lakin57@gmail.com', '206 Pearl Street', '206 Pearl Street', '2015-12-11', 'female'),
      ('Jeff Raynor', 'Jeff_Raynor33@gmail.com', '695 W Walnut Street', '695 W Walnut Street', '1997-04-05', 'male'),
      ('Jace Bradtke', 'Jace_Bradtke@yahoo.com', '8134 The Dell', '8134 The Dell', '2023-01-11', 'male'),
      ('Luz Bartell', 'Luz.Bartell@yahoo.com', '338 MacGyver Park', '338 MacGyver Park', '2020-05-04', 'male'),
      ('Sonia Okuneva', 'Sonia_Okuneva@hotmail.com', '808 Heidenreich Centers', '808 Heidenreich Centers', '1980-10-06', 'male'),
      ('Skye Tillman', 'Skye.Tillman55@yahoo.com', '89244 Alice Passage', '89244 Alice Passage', '1982-10-03', 'male'),
      ('Rachael Nikolaus', 'Rachael_Nikolaus@gmail.com', '9962 Robel Fords', '9962 Robel Fords', '2007-07-21', 'male'),
      ('Roma Terry-Collier', 'Roma_Terry-Collier26@gmail.com', '98370 Kirlin Views', '98370 Kirlin Views', '1975-01-09', 'male'),
      ('Roman Fritsch-Schaefer', 'Roman.Fritsch-Schaefer66@gmail.com', '1422 Marsh Lane', '1422 Marsh Lane', '1980-11-19', 'male'),
      ('Missouri Wiza', 'Missouri.Wiza@yahoo.com', '7048 Balistreri Motorway', '7048 Balistreri Motorway', '1989-07-28', 'female'),
      ('Marcellus Donnelly', 'Marcellus.Donnelly@gmail.com', '16253 Church Walk', '16253 Church Walk', '2011-01-29', 'female'),
      ('Margret Champlin', 'Margret.Champlin34@yahoo.com', '871 Mohamed Flat', '871 Mohamed Flat', '1980-04-22', 'female'),
      ('Christ Quitzon', 'Christ_Quitzon59@gmail.com', '26798 West Avenue', '26798 West Avenue', '1988-05-12', 'male'),
      ('Chester Kshlerin', 'Chester.Kshlerin@hotmail.com', '3129 Lavon Junctions', '3129 Lavon Junctions', '2003-11-08', 'female'),
      ('Joshua Torphy', 'Joshua.Torphy@hotmail.com', '2920 Steuber Lakes', '2920 Steuber Lakes', '1979-08-18', 'male'),
      ('Kelly Kilback', 'Kelly_Kilback33@yahoo.com', '1527 Willms Square', '1527 Willms Square', '2021-05-09', 'male'),
      ('Delpha Berge-Bernhard', 'Delpha.Berge-Bernhard@hotmail.com', '361 N 4th Street', '361 N 4th Street', '1990-11-03', 'male'),
	('Customer Customer', 'Kelly_Kilback33@yahoo.com', '1527 Willms Square', '1527 Willms Square', '2021-05-09', 'male');


-- Insert Statements for Administrator
insert into Administrator (adminName, adminEmail, dateHired)
VALUES('admin', 'admin@example.com', '1994-12-12'),
      ('Wallace Huel', 'Wallace_Huel@yahoo.com', '2002-07-23'),
      ('Leopoldo Rowe', 'Leopoldo_Rowe@yahoo.com', '2009-12-12'),
      ('Morris Erdman', 'Morris_Erdman76@gmail.com', '2001-08-26'),
      ('Dangelo McLaughlin', 'Dangelo_McLaughlin84@hotmail.com', '1987-10-12'),
      ('Rosalia Kub', 'Rosalia_Kub9@yahoo.com', '2001-07-06');


-- Insert Statements for UserPassword
insert into UserPassword (customerID, adminID, userAccountName, userEncryptedPassword, passwordExpiryTime, userAccountExpiryDate)
VALUES(1, null, 'Rusty.Romaguera', 'qmJrzCKwNUzH8h_', 100, '2024-12-10'),
      (2, null, 'Estell_Hayes45', 'qpZhU97omxJOeJK', 100, '2024-12-13'),
      (3, null, 'Berry62', '7V77rVJbOUpkf5_', 100, '2024-05-08'),
      (4, null, 'Merlin_Toy90', 'scu0gZYMNQxv1D5', 100, '2025-01-25'),
      (5, null, 'Kailee.Hagenes82', 'pmpuEtaZe23PO5Q', 100, '2024-04-03'),
      (6, null, 'Imani10', '8OC2OrOIrzeOOhO', 100, '2024-12-26'),
      (7, null, 'Kaylie84', 'sZN4A5ykaTuCmDR', 100, '2024-12-09'),
      (8, null, 'Merritt2', '7Ld28f0Q2rsawso', 100, '2024-05-09'),
      (9, null, 'Francisco_Sporer', '5a3LYrPGkGAbMHY', 100, '2024-09-27'),
      (10, null, 'Wyatt.Lindgren21', 'rtFwe1kSV7tl87H', 100, '2024-11-02'),
      (11, null, 'Chyna.Medhurst83', 'x_U18N7yKq63ihM', 100, '2024-09-14'),
      (12, null, 'Waylon3', 'dMtJsmnbNvzxUH4', 100, '2024-08-09'),
      (13, null, 'Clay_Daugherty', 'it00Sise4AP5J1k', 100, '2024-07-11'),
      (14, null, 'Yoshiko_Blanda', 'Sxduzs091zSs8eD', 100, '2024-09-29'),
      (15, null, 'Larissa.Hermiston32', 'E2dDfxl_C06ilcS', 100, '2024-05-03'),
      (16, null, 'Lilla74', 'La8ufOMwRffr0rw', 100, '2025-03-18'),
      (17, null, 'Jessica_Collins', 'Um8bEYEfJ3c_21p', 100, '2024-10-29'),
      (18, null, 'Judge46', '13ZZ50ehSLw88kv', 100, '2025-01-06'),
      (19, null, 'King_Legros', 'TUqKDVll7FIo7J6', 100, '2025-01-13'),
      (20, null, 'Yoshiko_Baumbach', 'YB9LMKRK9PpQsmX', 100, '2024-05-07'),
	(21, null, 'customer', 'customer', 100, '2024-12-10'),
      (null, 1, 'admin', 'admin', 100, '2024-10-24'),
      (null, 2, 'Haley_Koch6', '0bOcGEVNwNgKqhs', 100, '2024-11-02'),
      (null, 3, 'Melvin.Gerlach', '6pVr_nlEQykO5wf', 100, '2024-12-25'),
      (null, 4, 'Donato.Hickle41', 'rXaqemev_Oe0HbX', 100, '2024-09-28'),
      (null, 5, 'Giovanna70', 'Zmt05SuKspA6STu', 100, '2025-03-10'),
      (null, 6, 'Lucas_Koepp', 'O00OnCbPZMgcItH', 100, '2025-03-02');


-- Insert Statements for AboutUs
insert into AboutUs (companyAddress, companyShippingPolicy, companyReturnPolicy, companyContactInfo, companyBusinessDescription, managerID)
VALUES('93854 Lenore Springs', 'At iClothing, we strive to deliver your order swiftly and efficiently. We offer worldwide shipping options to ensure that our customers can enjoy our curated selection of luxury fashion wherever they are. Shipping costs and delivery times may vary depending on your location and the selected shipping method. For more details, please contact our customer service team for assistance.', 'Your satisfaction is our priority at iClothing. If for any reason you are not completely satisfied with your purchase, you may return it within 30 days of receipt for a full refund or exchange. Items must be returned in their original condition, unworn, with tags attached, and in their original packaging. ', 'Contact Us @ Info', 'Welcome to iClothing, your premier destination for luxury fashion from the world''s most renowned brands. At iClothing, we believe that fashion is not just about clothing; it''s a statement, an expression of individuality, and a reflection of personal style. We strive to curate a diverse selection of high-quality apparel for women, men, and kids, ensuring that everyone can find something that resonates with their unique tastes.', 2);


-- Insert Statements for UserQuery
insert into UserQuery (queryDate, queryDescription, customerID)
VALUES('2023-06-16', 'I received my order, but one item seems to be missing. Can you help me track it or provide assistance in resolving the issue?', 10),
      ('2024-02-12', 'Do you have any information about upcoming sales or promotions? I''m eyeing a few items but would like to wait for a possible discount.', 1),
      ('2023-10-22', 'Can you provide more details about the material used in the Dolce & Gabbana sweater I''m interested in? I want to make sure it''s suitable for the upcoming season.', 20),
      ('2023-12-11', 'Do you offer international shipping? I''m located outside the United States and would like to know if I can still place an order.', 15),
      ('2023-11-23', 'What is your policy regarding returns and exchanges? I want to make sure I understand the process before making a purchase.', 19),
      ('2023-08-17', 'Can you tell me if the Versace dress I''m interested in is true to size, or should I consider sizing up/down?', 9),
      ('2023-05-31', 'I''m looking for a specific Nike shoe model, but I couldn''t find it on your website. Can you assist me in locating it or suggest a similar alternative?', 6),
      ('2023-06-23', 'I''m interested in purchasing a Gucci handbag from iClothing, but I''m unsure about the size. Can you provide measurements or guidance on selecting the right size?', 4),
      ('2023-09-21', 'I''m shopping for my son and daughter and would like to know if you have any special promotions or discounts for children''s clothing?', 8),
      ('2024-03-12', 'I''m considering purchasing a Versace jacket as a gift. Do you offer gift wrapping services, and can I include a personalized message?', 7);


-- Insert Statements for Department
insert into Department (departmentName, departmentDescription)
VALUES('Women''s', 'Shop Women''s Clothing.'),
      ('Mens', 'Shop Men''s Clothing.'),
      ('Kids', 'Shop Kid''s Clothing.');


-- Insert Statements for Category
insert into Category (categoryName, categoryDescription, parentCategoryID, parentDepartmentID)
VALUES('Women''s Shirts', 'Shop Women''s Shirts', null, 1),
		('Men''s Shirts', 'Shop Men''s Shirts', null, 2),
		('Kid''s Shirts', 'Shop Kid''s Shirts', null, 3),
        ('Women''s Activewear', 'Shop Women''s Activewear', null, 1),
		('Men''s Activewear', 'Shop Men''s Activewear', null, 2),
		('Kid''s Activewear', 'Shop Kid''s Activewear', null, 3),
        ('Women''s Jeans', 'Shop Women''s Jeans', null, 1),
		('Men''s Jeans', 'Shop Men''s Jeans', null, 2),
		('Kid''s Jeans', 'Shop Kid''s Jeans', null, 3),
        ('Women''s Swimwear', 'Shop Women''s Swimwear', null, 1),
		('Men''s Swimwear', 'Shop Men''s Swimwear', null, 2),
		('Kid''s Swimwear', 'Shop Kid''s Swimwear', null, 3),
        ('Women''s Pants', 'Shop Women''s Pants', null, 1),
		('Men''s Pants', 'Shop Men''s Pants', null, 2),
		('Kid''s Pants', 'Shop Kid''s Pants', null, 3),
        ('Women''s Sweaters', 'Shop Women''s Sweaters', 1, null),
		('Men''s Sweaters', 'Shop Men''s Sweaters', 2, null),
		('Kid''s Sweaters', 'Shop Kid''s Sweaters', 3, null),
        ('Women''s Graphic Tees', 'Shop Women''s Graphic Tees', 1, null),
		('Men''s Graphic Tees', 'Shop Men''s Graphic Tees', 2, null),
		('Kid''s Graphic Tees', 'Shop Kid''s Graphic Tees', 3, null);


-- Insert Statements for Brand
insert into Brand (brandName, brandDescription)
VALUES('Dolce & Gabbana', null),
      ('Versace', null),
      ('GUCCI', null),
      ('Calvin Klein', null),
      ('Nike', null);


-- Insert Statements for Product
insert into Product (productName, productDescription, productPrice, productQty, categoryID, brandID)
VALUES('Crochet Crop Top', 'Lilac crochet crop top handmade in Italy. ', 1325, 2, 16, 1),
      ('Denim and Silk Banana Tree Print Jeans', 'Loose fit, multi-colored, made in Italy', 1925, 5, 8, 1),
	  ('5-Pocket Denim Jeans', 'Children''s 5-pocket cotton denim jeans with logo tag.', 355, 5, 9, 1),
      ('Medusa Baby Sleepsuit', 'This lovely piece for the littles ones features lace trims and a check print enhanced with a reimagined Medusa logo which appears in a repeated motif.', 205, 8, 18, 2),
      ('Medusa ''95 Flared Pants', 'These formal pants are made from a stretch wool blend in a flared silhouette.', 950, 3, 13, 2),
      ('Year of the Dragon T-Shirt', 'An oversized short-sleeved jersey T-shirt featuring a graphic embroidery and logo design for the Year of the Dragon.', 950, 2, 19, 2),
      ('Wool Twill Formal Pants', 'These wide-leg formal pants are crafted from virgin wool and are detailed with a tonal tailoring label at the back and welt pockets.', 1225, 3, 14, 2),
	  ('Faded Monogram Logo Crewneck T-Shirt', 'Made from 100% cotton, this slim fit t-shirt is designed for comfortable wear.', 39, 2, 20, 4),
	  ('Sportswear Club Fleece Cargo Shorts', 'Made with lightweight fleece that''s smooth on the outside and brushed soft on the inside, they''re an easy pick when you want a little extra warmth.', 40, 6, 15, 5),
	  ('Dri-FIT Legend T-Shirt', 'Nike''s Legend tee is made for all athletes from all-day play to gearing up for practice.', 25, 10, 6, 5),
	  ('Light-Support Non-Padded Longline Sports Bra', 'Lightweight, ribbed InfinaSoft fabric offers an irresistible softness that you can feel with every bend, stretch and shift so you can stay comfortable from morning to night.', 60, 9, 4, 5),
	  ('AeroSwift Dri-FIT ADV Running Pants', 'Designed for racing, the slim fit design uses our most innovative technologies to help you reach your goals.', 125, 7, 5, 5),
	  ('Swim Retro Flow Girls T-Back One-Piece Swimsuit', 'With bold colors and heritage Nike style, you’re sure to make a splash when rocking this swimsuit to the beach or pool. Designed specifically for young athletes, it features a T-back design that allows for easy movement.', 46, 4, 12, 5),
	  ('GG Polyester Tailored Pant', 'Double-breasted suits outline a classic silhouette in the men''s Spring Summer 2024 collection.', 1350, 3, 14, 3),
	  ('Peter Rabbit X Gucci T-Shirt', 'A playful print decorates this 100% cotton jersey T-shirt.', 240, 7, 21, 3),
	  ('Children''s Wool Sweater With GG', 'Crafted from blue felted wool, this bold sweater reveals a tonal stitched GG motif.', 480, 8, 18, 3),
	  ('GG Stretch Jersey Swimsuit', 'Inspired by the summer spirit and beach clubs on the Italian coast, this item is part of Gucci Lido. Asymmetric cut-out details make this perfect for warm-weather destinations.', 790, 5, 10, 3),
	  ('Nike Swim Men''s 7in Volley Shorts', 'Get ready for your ultimate day at the pool. They feature a stretchy waistband with an exterior drawcord to help you stay comfortable all day long.', 66, 6, 11, 5),
	  ('90s Loose Fit Jeans', 'Cut in a loose fit, these jeans are a relaxed style perfect for casual outfits. Fitted with a high rise waist and made with classic 5-pocket styling.', 89, 4, 7, 4),
	  ('Tech Knit Quarter Zip Sweater', 'Woven with a half milano knit, this Calvin Klein pullover sweater features a standout rib-knit neck and a smooth body.', 149, 5, 17, 4);

-- Insert Statements for UserComments
insert into UserComments (commentDate, commentDescription, customerID)
VALUES('2023-09-22', 'Great selection of men''s clothing! Ordered a few shirts from Nike, and they''re all fantastic quality. Highly recommend iClothing.', 3),
      ('2023-09-19', 'Impressed with the fast delivery of my Versace jeans from iClothing. The packaging was secure, and the jeans fit perfectly.', 3),
      ('2023-10-24', 'The shipping time for my Calvin Klein hoodie was longer than expected, but the product itself was worth the wait.', 19),
      ('2023-07-21', 'iClothing''s attention to detail is commendable!', 6),
      ('2024-01-01', 'The pricing at iClothing seems a bit steep compared to other retailers. While I love the quality of the Nike shirts I purchased, I wish they were more competitively priced.', 6),
      ('2023-05-06', 'My Calvin Klein jeans were stylish and fit perfectly!', 16),
      ('2023-04-18', 'The selection at iClothing is impressive, but I wish they offered more variety in their kid''s clothing section.', 5),
      ('2024-01-17', 'iClothing''s commitment to customer satisfaction is evident! Had a minor issue with my order, but their responsive customer service team resolved it swiftly.', 15),
      ('2024-02-13', 'The checkout process at iClothing was smooth, but I wish they offered more payment options. ', 11),
      ('2024-03-13', 'Mixed feelings about iClothing''s return policy. While I appreciate the 30-day window, the process felt a bit cumbersome. Nonetheless, happy with my Dolce & Gabbana blouse.', 9);


-- Insert Statements for ShoppingCart
insert into ShoppingCart (customerID)
VALUES(1),
      (2),
      (3),
      (4),
      (5),
      (6),
      (7),
      (8),
      (9),
      (10),
      (11),
      (12),
      (13),
      (14),
      (15),
      (16),
      (17),
      (18),
      (19),
      (20);


-- Insert Statements for ItemWrapper
insert into ItemWrapper (cartID, productID, productQty)
VALUES(1, 6, 1),
      (1, 2, 1),
      (3, 3, 1),
      (3, 1, 1),
      (4, 2, 2),
      (6, 2, 2),
      (6, 4, 2),
      (7, 5, 1),
      (7, 6, 1),
      (7, 2, 1),
      (8, 3, 1),
      (11, 1, 2),
      (12, 2, 1),
      (13, 6, 1),
      (13, 2, 2),
      (14, 4, 1),
      (15, 1, 2),
      (15, 3, 2),
      (15, 2, 2),
      (15, 5, 1),
      (15, 4, 1),
      (19, 4, 2),
      (20, 6, 2);


-- Insert Statements for OrderStatus
insert into OrderStatus (status, statusDate, cartID)
VALUES('paid', '2024-02-25', 1),
      ('none', '2024-03-01', 2),
      ('shipped', '2024-03-17', 3),
      ('delivered', '2024-01-21', 4),
      ('none', '2024-01-26', 5),
      ('paid', '2024-03-22', 6),
      ('delivered', '2024-03-05', 7),
      ('paid', '2024-03-23', 8),
      ('none', '2024-03-04', 9),
      ('none', '2024-01-23', 10),
      ('none', '2024-04-01', 11),
      ('none', '2024-03-17', 12),
      ('none', '2024-02-22', 13),
      ('delivered', '2024-02-17', 14),
      ('paid', '2024-03-01', 15),
      ('none', '2024-01-22', 16),
      ('none', '2024-01-23', 17),
      ('none', '2024-03-03', 18),
      ('none', '2024-02-23', 19),
      ('delivered', '2024-03-30', 20);


-- Insert Statements for Email
insert into Email (emailDate, emailSubject, emailBody, customerID, adminID)
VALUES('2024-04-02', 'Confirmation of International Shipping Availability', 'Thank you for reaching out to iClothing regarding our shipping policies. We''re delighted to inform you that we do offer international shipping to customers worldwide.', 7, 6),
      ('2024-03-12', 'Information on Material Composition for Your Dolce & Gabbana Sweater', 'Our Dolce & Gabbana sweaters typically feature a blend of high-quality materials, including wool, cashmere, and cotton. However, specific compositions may vary depending on the style and design.', 9, 2),
      ('2024-04-01', 'Resolving the Issue of the Missing Item in Your iClothing Order', 'Rest assured, we are committed to resolving this issue promptly for you. To assist us in investigating further and expediting the process, could you please provide us with the order number and the specific item that is missing?', 14, 6),
      ('2024-03-28', 'Help Finding Your Desired Nike Shoe Model', 'To better assist you, could you please provide us with the specific Nike shoe model you''re looking for? Alternatively, if you''re open to exploring similar alternatives, our team can offer recommendations based on your preferences.', 14, 5),
      ('2024-03-05', 'Exclusive Updates on Upcoming Promotions at iClothing', 'We''re excited to share that we have some exciting promotions and discounts lined up for our customers, including special offers on selected items and upcoming sales events.', 7, 5),
      ('2024-02-29', 'Thank You for Your Positive Feedback!', 'We wanted to take a moment to express our heartfelt gratitude for your recent feedback regarding your shopping experience with iClothing. Your kind words truly mean the world to us, and we''re delighted to hear that you''re satisfied with your purchase.', 18, 1),
      ('2024-03-27', 'Adding a Personalized Touch to Your iClothing Gift Order', 'To add a personalized touch to your order, we offer complimentary gift wrapping services along with the option to include a personalized message. If you would like to include a personalized message with your gift, please provide us with the message you''d like to convey, and we''ll ensure it''s included with your order.', 18, 6),
      ('2024-03-14', 'Addressing Your Concerns About Pricing at iClothing', 'We understand that pricing plays a significant role in the overall shopping experience, and we continuously strive to offer competitive prices while maintaining the exceptional quality and service that our customers expect from us. However, we acknowledge that there may be instances where our pricing may not fully meet your expectations.', 6, 6),
      ('2024-03-19', 'Guidance for Navigating the iClothing Website', 'If you''re searching for specific items, we recommend using our search bar or browsing through our categories and filters to narrow down your options. Additionally, our customer service team is available to assist you with any specific inquiries or product recommendations you may have.', 6, 5),
      ('2024-03-07', 'Your iClothing Order is Being Processed!', 'Our team is diligently working to prepare your order for shipment, and you can expect to receive a confirmation email with tracking details once your order has been dispatched.', 14, 1),
      ('2024-03-12', 'Confirmation of Return Processing for Your iClothing Order', 'Your refund has been processed back to the original payment method used for the purchase. Depending on your financial institution, it may take several business days for the refund to reflect in your account.', 8, 3),
      ('2024-03-21', 'Share Your Feedback with iClothing!', 'We invite you to participate in a brief customer satisfaction survey to share your thoughts and opinions about your recent experience with iClothing. Your feedback will help us identify areas for improvement and ensure that we continue to meet and exceed your expectations.', 9, 2),
      ('2024-02-27', 'Assistance with Your Product Inquiry', 'To provide you with accurate information, could you please provide us with more details about the specific product you''re interested in? This could include the product name, model number, or any other relevant information that will help us better understand your inquiry.', 19, 3),
      ('2024-03-19', 'Exclusive Discount Offer for Valued Customers', 'As a token of our appreciation for your continued support, we''re delighted to offer you an exclusive discount on your next iClothing purchase!', 4, 2);


-- Insert Statements for ItemDelivery
insert into ItemDelivery (stickerDate, cartID)
VALUES('2024-03-28', 1),
      ('2024-03-31', 2),
      ('2024-03-28', 3),
      ('2024-03-22', 4),
      ('2024-03-14', 6),
      ('2024-03-06', 7),
      ('2024-03-22', 8),
      ('2024-02-27', 10),
      ('2024-03-04', 14),
      ('2024-03-15', 15),
      ('2024-03-24', 16),
      ('2024-03-02', 17),
      ('2024-03-07', 18),
      ('2024-03-24', 20);
