-- Rode conectado ao banco DeveloperEvaluation (NÃO ao postgres)
-- Ex.: docker exec -i pg-sales psql -U postgres -d DeveloperEvaluation -v ON_ERROR_STOP=1 -f - < .\inserts.sql

BEGIN;

-- ===== USERS =====
INSERT INTO "Users" ("Id","Username","Password","Phone","Email","Status","Role")
SELECT gen_random_uuid(), 'admin', '123', '(11) 99999-9999', 'admin@example.com', 'Active', 'Admin'
WHERE NOT EXISTS (SELECT 1 FROM "Users" WHERE "Username" = 'admin');

-- (exemplo opcional de outro usuário)
INSERT INTO "Users" ("Id","Username","Password","Phone","Email","Status","Role")
SELECT gen_random_uuid(), 'user', '123', '(11) 98888-7777', 'user@example.com', 'Active', 'User'
WHERE NOT EXISTS (SELECT 1 FROM "Users" WHERE "Username" = 'user');

-- ===== SALES =====
-- S-001
INSERT INTO "Sales" ("Id","SaleNumber","SaleDate","ClientName","Branch","TotalAmount","Status","CreatedAt","UpdatedAt")
SELECT 'd95c7b41-e509-43cb-9b0e-1f372cae7dc8', 'S-001', now() - interval '2 days', 'Cliente A', 'Filial 01', 1, 1, now(), NULL
WHERE NOT EXISTS (SELECT 1 FROM "Sales" WHERE "SaleNumber" = 'S-001');

-- S-002
INSERT INTO "Sales" ("Id","SaleNumber","SaleDate","ClientName","Branch","TotalAmount","Status","CreatedAt","UpdatedAt")
SELECT '48a3def1-f4e4-4d9a-8632-59c9764965ac', 'S-002', now() - interval '1 day', 'Cliente B', 'Filial 02', 1, 1, now(), NULL
WHERE NOT EXISTS (SELECT 1 FROM "Sales" WHERE "SaleNumber" = 'S-002');

-- ===== SALE ITEMS =====
-- Itens para S-001
INSERT INTO "SaleItems" ("Id","ProductName","Quantity","UnitPrice","Discount","SaleId")
SELECT gen_random_uuid(), 'Skol Lata 350ml', 4, 4.50, 4*4.50*0.10, s."Id"
FROM "Sales" s
WHERE s."SaleNumber" = 'S-001'
  AND NOT EXISTS (
    SELECT 1 FROM "SaleItems" si
    WHERE si."SaleId" = s."Id" AND si."ProductName" = 'Skol Lata 350ml'
  );

INSERT INTO "SaleItems" ("Id","ProductName","Quantity","UnitPrice","Discount","SaleId")
SELECT gen_random_uuid(), 'Brahma Duplo Malte 600ml', 10, 8.90, 10*8.90*0.20, s."Id"
FROM "Sales" s
WHERE s."SaleNumber" = 'S-001'
  AND NOT EXISTS (
    SELECT 1 FROM "SaleItems" si
    WHERE si."SaleId" = s."Id" AND si."ProductName" = 'Brahma Duplo Malte 600ml'
  );

-- Itens para S-002
INSERT INTO "SaleItems" ("Id","ProductName","Quantity","UnitPrice","Discount","SaleId")
SELECT gen_random_uuid(), 'Guaraná Antarctica 2L', 3, 9.50, 0, s."Id"
FROM "Sales" s
WHERE s."SaleNumber" = 'S-002'
  AND NOT EXISTS (
    SELECT 1 FROM "SaleItems" si
    WHERE si."SaleId" = s."Id" AND si."ProductName" = 'Guaraná Antarctica 2L'
  );

INSERT INTO "SaleItems" ("Id","ProductName","Quantity","UnitPrice","Discount","SaleId")
SELECT gen_random_uuid(), 'Stella Artois Long Neck 330ml', 15, 6.50, 15*6.50*0.20, s."Id"
FROM "Sales" s
WHERE s."SaleNumber" = 'S-002'
  AND NOT EXISTS (
    SELECT 1 FROM "SaleItems" si
    WHERE si."SaleId" = s."Id" AND si."ProductName" = 'Stella Artois Long Neck 330ml'
  );

COMMIT;
