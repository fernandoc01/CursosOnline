import SesionUsuarioReducer from './SesionUsuarioReducer'
import openSnackBarReducer from './openSnackBarReducer'

export const mainReducer = ({sesionUsuario, openSnackBar}, action) => {
    return {
        sesionUsuario : SesionUsuarioReducer(sesionUsuario, action),
        openSnackBar : openSnackBarReducer(openSnackBar, action)
    }
}