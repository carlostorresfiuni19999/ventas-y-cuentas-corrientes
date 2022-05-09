import React from 'react'

function FDPControl(props){
    switch(props.fdp){
        case 'CONTADO':
            return(
                <div>
                    <p>El pago se realizara en contado, por lo que no se pide que ingrese la cantidad de cuotas a pagar</p>
                    <label htmlFor='cantCuotas'>Cantidad de Cuotas:</label>
                    <input type='number' className='form-control form-control-sm' defaultValue={1} min={1} max={10} id='cantCuotas' disabled/>
                    

                </div>
            )
        case 'CREDITO':
            return(
                <div>
                    <p>El pago se realizara en credito, por lo que se pide que ingrese la cantidad de cuotas a pagar</p>
                    <label htmlFor='cantCuotas'className=''>Cantidad de Cuotas:</label>
                    <input type='number' className='form-control form-control-sm' defaultValue={1} min={1} max={10} id='cantCuotas' />
                </div>
            )
        default:
            alert('ocurrio un error inesperado!!!')
            break;
    }
}

export default FDPControl