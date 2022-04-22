import React from 'react'
import Input from './Input'

function FormikControl(props){
    const {control, ...resto} = props
    switch(control){
        case 'input':
            return <Input {...resto} />
        default:
            return null;
    }
}

export default FormikControl