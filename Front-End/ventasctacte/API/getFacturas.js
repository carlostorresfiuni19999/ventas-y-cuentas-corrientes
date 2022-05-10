export async function getFacturas(token) {
    const myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${token}` );

    const requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
        
    };

    return fetch("https://cuentasctacte-web-api20220425205158.azurewebsites.net/api/Facturas", requestOptions)
}

export default getFacturas