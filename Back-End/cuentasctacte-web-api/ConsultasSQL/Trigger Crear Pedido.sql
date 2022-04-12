create trigger guardarPedido
on dbo.pedidoDetalles
after insert
AS
    BEGIN TRANSACTION
    DECLARE 
        @id_pedido int, 
        @id_producto int,
        @cantidad_producto int,
        @cantidad_facturada int

    if  NOT exists (select dbo.Pedidos.Id from Pedidos where Pedidos.Id = (
        select inserted.IdPedido from Inserted
    ))
    BEGIN
        RAISERROR('No existe el pedido al cual se hace referencia',16,1)
        ROLLBACK TRANSACTION
    END

    set @id_pedido = (select dbo.Pedidos.Id from Pedidos where Pedidos.Id = (
        select inserted.IdPedido from Inserted
    ))

    if NOT exists (select dbo.Productos.Id from Productos where Productos.Id = (
        select Inserted.idProducto from inserted  
    ))
    BEGIN
        RAISERROR('El producto al cual se hace referencia no existe', 16,1)
        ROLLBACK TRANSACTION
    END

    set @id_producto =  (select dbo.Productos.Id from Productos where Productos.Id = (
        select Inserted.idProducto from inserted  
    ))

    if (select inserted.cantidadProducto from inserted) <= 0 
    BEGIN
        RAISERROR('La cantidad de productos insertados debe ser mayor a 0', 16, 1)
        ROLLBACK TRANSACTION
    END

    set @cantidad_producto = (select inserted.cantidadProducto from inserted)

    if (select inserted.cantidadFacturada from inserted ) < 0
    BEGIN
        RAISERROR('La cantidad Facturada debe ser por lo menos 1', 16,1 )
        ROLLBACK TRANSACTION
    END

    set @cantidad_facturada = (select inserted.CantidadFacturada from inserted)

    if(@cantidad_facturada = 1)
    BEGIN
        
    END


    

GO