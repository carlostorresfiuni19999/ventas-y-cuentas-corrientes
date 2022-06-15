export async function getFacturasDePedidos(token, id) {
    const myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${token}` );

    const requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
        
    };

    return fetch(`https://cuentasctacte-web-api20220425205158.azurewebsites.net/api/Pedidos/FacturasDelPedido?id_Pedido=${id}`, requestOptions)
}

export default getFacturasDePedidos