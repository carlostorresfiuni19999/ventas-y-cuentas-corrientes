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

    return fetch(link, requestOptions);
}
