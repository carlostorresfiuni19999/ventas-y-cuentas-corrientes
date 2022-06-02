export async function getFactura(token, id) {
    const myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${token}` );

    const link = `https://cuentasctacte-web-api20220425205158.azurewebsites.net/api/Facturas/${id}`
    const requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
        
    };

    return fetch(link, requestOptions)
        
}

export default getFactura