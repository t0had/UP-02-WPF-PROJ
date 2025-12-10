use AdsServiceDB;

go

INSERT INTO Users (User_Login, User_Password)
SELECT DISTINCT User_Login, User_Password FROM Ads_Data;

INSERT INTO Cities (Name)
SELECT DISTINCT City FROM Ads_Data;

INSERT INTO Categories (Name)
SELECT DISTINCT Category FROM Ads_Data;

INSERT INTO Ad_Types(Name)
SELECT DISTINCT Ad_Type FROM Ads_Data;

INSERT INTO Ad_Statuses(Name)
SELECT DISTINCT Ad_Status FROM Ads_Data;

INSERT INTO Advertisements(user_id, Ad_Title, Ad_Description, Ad_Post_Date, city_id, category_id, ad_type_id, ad_status_id, Price)
SELECT
(SELECT TOP 1 ID FROM Users WHERE Users.User_Login = Ads_Data.User_Login),
Ad_Title,
Ad_Description,
Ad_Post_Date,
(SELECT TOP 1 ID FROM Cities WHERE Cities.Name = Ads_Data.City),
(SELECT TOP 1 ID FROM Categories WHERE Categories.Name = Ads_Data.Category),
(SELECT TOP 1 ID FROM Ad_Types WHERE Ad_Types.Name = Ads_Data.Ad_Type),
(SELECT TOP 1 ID FROM Ad_Statuses WHERE Ad_Statuses.Name = Ads_Data.Ad_Status),
Price
FROM Ads_Data;