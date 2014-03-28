DECLARE @UserId nvarchar(128)
SELECT @UserId = Id FROM AspNetUsers WHERE UserName = 'jsmith'

INSERT INTO AspNetUserRoles
(UserId, RoleId)
VALUES (@UserId, 'Demographics')

INSERT INTO AspNetUserRoles
(UserId, RoleId)
VALUES (@UserId, 'Financial')

SELECT @UserId = Id FROM AspNetUsers WHERE UserName = 'djohnson'

INSERT INTO AspNetUserRoles
(UserId, RoleId)
VALUES (@UserId, 'Financial')

SELECT @UserId = Id FROM AspNetUsers WHERE UserName = 'ariley'

INSERT INTO AspNetUserRoles
(UserId, RoleId)
VALUES (@UserId, 'OnlineAccess')


