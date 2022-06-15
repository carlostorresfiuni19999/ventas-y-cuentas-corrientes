import PersonFormUpdate from "./PersonFormUpdate";
import PersonFormPost from "./PersonFormPost";

const PersonForm = ({onEdit, onSave, editable = false, value}) => {

 if(editable){
    return <PersonFormUpdate 
        onEdit={onEdit}
        editable={editable}
        value={value} />
 } else {
    return <PersonFormPost
        onSave ={onSave} 
        />
        
 }
}

export default PersonForm;