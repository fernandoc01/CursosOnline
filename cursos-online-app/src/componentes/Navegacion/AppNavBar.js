import React from 'react'
import {AppBar} from '@material-ui/core'
import BarSesion from './Bar/BarSesion'

const AppNavBar = () => {
    return(
        <AppBar position="static">
            <BarSesion/>
        </AppBar>
    )
}

export default AppNavBar