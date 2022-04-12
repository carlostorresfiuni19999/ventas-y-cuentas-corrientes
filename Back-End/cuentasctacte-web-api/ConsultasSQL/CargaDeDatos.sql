select * from Productos
select * from Depositos

--Para insertar Productos
insert into Productos values(
    'La destructura',
    'Gaming La Destructora, 16Gb de RAM, Intel I9 12th Generacion',
    'Intel',
    '123400023423',
    1,
    12000000,
    1200000,
    0
    )

    --Para insertar Tipos de Personas
    insert into dbo.TipoPersonas values('Vendedor',0)
    insert into dbo.TipoPersonas values('Cliente', 0)

    --Para insertar Depositos
    insert into dbo.depositos values ('Deposito 1', 'Materia Prima', 0)
    insert into dbo.depositos values ('Deposito 2', 'Productos', 0)