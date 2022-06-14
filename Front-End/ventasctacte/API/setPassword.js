export default async function setPassword(token, UserName, raw){
    const myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${token}` );
    myHeaders.append("Content-Type", "application/json");

    const link = `https://cuentasctacte-web-api20220425205158.azurewebsites.net/api/Account/SetPassword?UserName=${UserName}`
    const requestOptions = {
        method: 'PUT',
        headers: myHeaders,
        body: raw,
        redirect: 'follow'
        
    };

    try {
        const response = await fetch(link, requestOptions);
        const result_1 = await response.text();
        if (result_1.ok) {
            alert("Modificado con exito");
        } else {
            alert("Error en la modificacion, Verifica las credenciales si son correctas");
        }
    } catch (message_1) {
        return console.log(message_1);
    }
}