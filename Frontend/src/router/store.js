import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(vuex);

const store = new Vuex.Store({
    state: {
        email: '',
        status: '',
        token: localStorage.getItem('Token') || ''
    },
    getters: {
        getEmail: state => {
            return state.email;
        },
        isLoggedIn: state => {
            return !!state.token;
        },
        isAuthenticated: state => {
            return state.status;
        }
    },
    mutations: {
        authentication_success(state, token, email){
            state.status = 'success'
            state.token = token
            state.email = email
        },
        authentication_invalid(state){
            state.status = 'unsuccessful'
        },
        logout(state){
            state.status = ''
            state.token = ''
            state.email = ''
        }
    }
})