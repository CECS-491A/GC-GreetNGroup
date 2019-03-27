<template>
    <div class="testlogin">
        <h1>{{ msg }}</h1>
        <h4>Login</h4>
        <input type="text" name="email address" v-model="input_email" placeholder="E-mail Address" required/>
        <input type="password" name="password" v-model="input_password" placeholder="Password" required/>
        <button type="button" v-on:click="login()">Login</button>
    </div>
</template>

<script>
/* eslint-disable */
import axios from 'axios'
export default {
    name: 'TestLogin',
    data() {
        return {
            msg: 'GreetNGroup',
            input_email: '',
            input_password: '',
            JWT: null
        }
    },
    methods : {
        login(){
            axios.post('http://localhost:50884/api/JWT/grant', {
            userInputUsername: input_email,
            userInputPassword: input_password
        }).then(response => {
            this.JWT = response.data.JWT
            localStorage.setItem('JWT', response.data.JWT)
            if(localStorage.getItem('JWT') != null){
                this.$router.push('/')
            }
        })}
    }
        
}
</script>

<style scoped>
h4 {
  font-weight: normal;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
