export async function getPersonas(token) {
    const myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${token}` );

    const requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
    };

    return fetch("https://localhost:44300/api/Personas", requestOptions)
        
}

export default getPersonas