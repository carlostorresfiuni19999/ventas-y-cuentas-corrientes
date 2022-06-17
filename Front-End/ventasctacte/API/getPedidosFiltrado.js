export async function getPedidosFiltrado(token, inicio, fin, estado) {
    const myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${token}` );

    const requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
        
    };

    return fetch(`https://cuentasctacte-web-api20220425205158.azurewebsites.net/api/Pedidos/PedidoReportes?Inicio=${inicio}&Fin=${fin}&estado=${estado}`, requestOptions)
}

export default getPedidosFiltrado