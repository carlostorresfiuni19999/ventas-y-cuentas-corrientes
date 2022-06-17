export default function getUsersDisponibles(){
    const token = JSON.parse(sessionStorage.getItem("token")).access_token;
    const myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${token}`);

    const requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
    };

    return fetch("https://cuentasctacte-web-api20220425205158.azurewebsites.net/api/Cajas/Personas/Disponibles", requestOptions)
}