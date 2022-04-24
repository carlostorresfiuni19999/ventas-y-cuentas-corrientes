import React from "react";
import LoginForm from "../components/LoginForm";
import axios from 'axios'

export default function prueba({ notas }) {
    return (
        <div>
            <h1>SSSSSSSS</h1>
            
        </div>
        
    )
}


prueba.getInitialProps = async () => {
    const res = await fetch('http://localhost:3000/api/lista')
    const data = await res.json()
    console.log(data)
    return { notas: data }
}