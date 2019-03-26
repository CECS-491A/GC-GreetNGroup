import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(vuex);

const store = new Vuex.Store({
    state: {
        status: '',
        token: localStorage.getItem('Token') || '',
        users: {
            
        }
    },
    getters: {
        getUser: state => {
            return state.users.filter(user => users.userName);
        },
        isLoggedIn: state => {
            return !!state.token;
        },
        isAuthenticated: state => {
            return state.status;
        }
    },
    mutations: {
        authentication_success(state, token, user){
            state.status = 'success'
            state.token = token
            state.user = user
        },
        authentication_invalid(state){
            state.status = 'unsuccessful'
        },
        logout(state){
            state.status = ''
            state.token = ''
        }
    }
})