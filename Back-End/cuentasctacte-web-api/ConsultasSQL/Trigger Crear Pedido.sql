select * from Stocks

select * from Personas

select * from Productos

insert into Personas VALUES('Carlos', 'Torres','+32 434 555', 'CI','3330432',20000000, 0, 0, 'carlost')

insert into Personas VALUES('Juan', 'Caceres','+11 324 155', 'CI','5939563',0, 0, 0, 'Juan.Vendedor@mail.com')

update Personas
set Personas.LineaDeCredito = 40000000
where Personas.Id = 7

select * from PedidoDetalles