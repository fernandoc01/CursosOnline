import Httpcliente from '../servicios/Httpcliente'

export const actionRegistrarUsuario = usuario => {
    return new Promise((resolve, reject) => {
        Httpcliente.post('/Usuario/registrar', usuario)
        .then(response => {
            resolve(response)
        })
    })
}

export const obtenerUsuarioActual = (dispatch) => {
    console.log("entre al action", dispatch)
    return new Promise((resolve, reject) => {
        Httpcliente.get('/Usuario')
        .then(response => {
            console.log("Response1", response)
            dispatch({
                type: "INICIAR_SESION",
                sesion: response.data,
                autenticado : true
            })
            resolve(response)
        })
    })
}

export const actualizarUsuario = usuario => {
    return new Promise((resolve, reject) => {
        Httpcliente.put('/Usuario', usuario)
        .then(response => {
            resolve(response)
        })
        .catch(error => {
            resolve(error.response)
        })
    })
}

export const loginUsuario = usuario => {
    return new Promise((resolve, reject) => {
        Httpcliente.post('/Usuario/login', usuario)
        .then(response => {
            resolve(response)
        })
    })
}