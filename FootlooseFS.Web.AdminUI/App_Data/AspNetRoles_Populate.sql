INSERT INTO AspNetRoles
(Id, Name)
VALUES 
('Admin', 'Admin'),
('Demographics', 'Demographics'),
('Financial', 'Financial'),
('OnlineAccess', 'OnlineAccess')

DECLARE @UserId nvarchar(128)
SELECT @UserId = Id FROM AspNetUsers WHERE UserName = 'admin'

INSERT INTO AspNetUserRoles
(UserId, RoleId)
VALUES (@UserId, 'Admin')