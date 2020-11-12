import Httpcliente from '../servicios/Httpcliente'
import axios from 'axios'

const instancia = axios.create()
instancia.CancelToken = axios.CancelToken
instancia.isCancel = axios.isCancel

export const actionRegistrarUsuario = usuario => {
    return new Promise((resolve, reject) => {
        instancia.post('/Usuario/registrar', usuario)
        .then(response => {
            resolve(response)
        }).catch(error => {
            resolve(error.response)
        })
    })
}

export const obtenerUsuarioActual = (dispatch) => {
    
    return new Promise((resolve, reject) => {
        Httpcliente.get('/Usuario')
        .then(response => {

            if(response.data && response.data.imagenPerfil){
                let fotoPerfil = response.data.imagenPerfil
                const nuevoFile = 'data:image/' + fotoPerfil.extension + ';base64,' + fotoPerfil.data
                response.data.imagenPerfil = nuevoFile
            }
         
            dispatch({
                type: "INICIAR_SESION",
                sesion: response.data,
                autenticado : true
            })
            resolve(response)
        })
        .catch(error => {
            resolve(error.response)
        })
    })
}

export const actualizarUsuario = (usuario, dispatch) => {
    return new Promise((resolve, reject) => {
        Httpcliente.put('/Usuario', usuario)
        .then(response => {
            
            if(response.data && response.data.imagenPerfil){
                let fotoPerfil = response.data.imagenPerfil
                const nuevoFile = 'data:image/' + fotoPerfil.extension + ';base64,' + fotoPerfil.data
                response.data.imagenPerfil = nuevoFile
            }

            console.log("response: ", response)
            dispatch({
                type: "INICIAR_SESION",
                sesion: response.data,
                autenticado: true
            })

            resolve(response)

        })
        .catch(error => {
            resolve(error.response)
        })
    })
}

export const loginUsuario = (usuario, dispatch) => {
    return new Promise((resolve, reject) => {
        instancia.post('/Usuario/login', usuario)
        .then(response => {

            if(response.data && response.data.imagenPerfil){
                let fotoPerfil = response.data.imagenPerfil
                const nuevoFile = 'data:image/' + fotoPerfil.extension + ';base64,' + fotoPerfil.data
                response.data.imagenPerfil = nuevoFile
            }
    
            dispatch({
                type: "INICIAR_SESION",
                sesion: response.data,
                autenticado: true
            })    

            resolve(response)
        })
        .catch(error => {
            resolve(error.response)
        })
    })
}