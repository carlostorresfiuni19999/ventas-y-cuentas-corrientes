export default function postCaja(raw, token){
    const myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${token}`);
    myHeaders.append("Content-Type", "application/json");

    const requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: raw,
        redirect: 'follow'
    };

    return fetch("https://cuentasctacte-web-api20220425205158.azurewebsites.net/api/Cajas", requestOptions);
        
}