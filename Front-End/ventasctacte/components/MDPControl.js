import React from 'react'

function MDPControl(props) {
    switch (props.mdp) {
        case 'EFECTIVO':
            return (
                <div>
                    <p>El pago se realizara en efectivo, Ingrese el monto que desee pagar:</p>
                    <h5>monto: </h5><input className='form-control form-control-sm' type='number' step={10000} defaultValue={100000} min={100000} max={props.montoMax}></input>


                </div>
            )
        case 'TARJETA':
            return (
                <div>
                    <p>El pago se realizara en tarjeta, Ingrese el monto que desee pagar:</p>
                    <input type='number' className='form-control form-control-sm' step={10000} defaultValue={100000} min={100000} max={props.montoMax}></input>
                </div>
            )
        case 'TRANSFERENCIA':
            return (
                <div>
                    <p>El pago se realizara en transferencia, Ingrese el monto que desee pagar:</p>
                    <input type='number' className='form-control form-control-sm' step={10000} defaultValue={100000} min={100000} max={props.montoMax}></input>
                </div>
            )
        default:
            alert('ocurrio un error inesperado!!!')
            break;
    }
}

export default MDPControl