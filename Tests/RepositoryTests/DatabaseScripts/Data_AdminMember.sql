-- Table: public.AdminMemberStatus

INSERT INTO public."AdminMemberStatus"(
    "Id", "Name")
VALUES (1, '啟用');
INSERT INTO public."AdminMemberStatus"(
    "Id", "Name")
VALUES (2, '停用');
INSERT INTO public."AdminMemberStatus"(
    "Id", "Name")
VALUES (3, '刪除');
INSERT INTO public."AdminMemberStatus"(
    "Id", "Name")
VALUES (4, '鎖定');

COMMIT;

-- Table: public.AdminMember

INSERT INTO public."AdminMember"(
    "Id", "Guid", "UserName", "Email", "Account", "Pwd", "StatusId", "ErrorTimes", "LastLoginDate", "IsMaster", "HashSalt", "ExpirationDate", "CreateDate", "UpdateDate")
VALUES (1, '936DA01F-9ABD-4d9d-80C7-02AF85C822A8', 'test', 'test@example.com', 'test', 'E0BEBD22819993425814866B62701E2919EA26F1370499C1037B53B9D49C2C8A', 1, 0, '2022-09-11 08:10:14.10736+00', true, '123', '2022-09-12 08:10:14.080766+00', '2021-11-19 13:16:24.006906+00', '2022-09-11 08:10:14.108783+00');
INSERT INTO public."AdminMember"(
    "Id", "Guid", "UserName", "Email", "Account", "Pwd", "StatusId", "ErrorTimes", "LastLoginDate", "IsMaster", "HashSalt", "ExpirationDate", "CreateDate", "UpdateDate")
VALUES (2, '12345678-1234-1234-1234-123456789012', 'test1', 'test1@example.com', 'test1', 'E0BEBD22819993425814866B62701E2919EA26F1370499C1037B53B9D49C2C8A', 1, 0, '2022-09-11 08:10:14.10736+00', true, '123', '2022-09-12 08:10:14.080766+00', '2021-11-19 13:16:24.006906+00', '2022-09-11 08:10:14.108783+00');

COMMIT;