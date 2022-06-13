export async function editPersona(token, userName, persona) {
    const myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${token}` );
    myHeaders.append("Content-Type", "application/json");

    const link = `https://cuentasctacte-web-api20220425205158.azurewebsites.net/api/Account/Edit?username=${userName}`;
    const requestOptions = {
        method: 'PUT',
        headers: myHeaders,
        body: JSON.stringify(persona),
        redirect: 'follow'
        
    };

    try {
        const response = await fetch(link, requestOptions);
        const result_1 = await response.text();
        console.log(result_1);
        alert("Editado con exito");
    } catch (error) {
        console.log(error);
    }
}
