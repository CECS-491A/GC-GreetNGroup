<template>
  <div class="UpdateProfile">
    <h1>Update Profile</h1>

    <br />
    <v-alert
      :value="message"
      dismissible
      type="success"
      transition="scale-transition"
    >
    {{message}}
    </v-alert>
    <v-alert
      :value="errorMessage"
      dismissible
      type="error"
      transition="scale-transition"
    >
    {{errorMessage}}
    </v-alert>
    
    <br />

    <v-text-field
            name="FirstName"
            id="FirstName"
            v-model="FirstName"
            type="text"
            label="First Name"/>
      <br />
      <v-text-field
            name="LastName"
            id="LastName"
            v-model="LastName"
            type="text"
            label="Last Name"/>
      <br />
      <v-text-field
            name="City"
            id="City"
            v-model="City"
            type="text"
            label="City"/>
      <br />
      <v-text-field
            name="State"
            id="State"
            v-model="State"
            type="text"
            label="State"/>
      <br />
      <v-text-field
            name="Country"
            id="Country"
            v-model="this.profile.Country"
            type="text"
            label="Country"/>
      <br />
      <v-btn color="success" v-on:click="UpdateProfile">Update Profile</v-btn>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'UpdateProfile',
  data () {
    return {
      errorMessage: null,
      profile: {
        FirstName: null,
        LastName: null,
        DoB: null,
        City: null,
        State: null,
        Country: null,
        JwtToken: null
      },
      message: null
    }
      },
  created () {
    axios({
      method: 'GET',
      url: 'https://api.greetngroup.com/api/user/update/getuser',
      headers: {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Credentials': true
      },
      data: {
        jwtToken = localStorage.getItem(jwtToken)
      }
    })
      .then(response => (this.profile = response.data))
      .catch(e => { this.errorMessage = e.response.data })
  },
  methods: {
    UpdateProfile: function (){
        axios({
      method: 'GET',
      url: 'https://api.greetngroup.com/api/user/update/getuser',
      headers: {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Credentials': true
      },
      data: {
        FirstName = this.profile.FirstName,
        LastName = this.profile.LastName,
        DoB = this.profile.DoB,
        City = this.profile.City,
        State = this.profile.State,
        Country = this.profile.Country,
        JwtToken = null
      }
    })
      .then(response => (this.message = response.data))
      .catch(e => { this.errorMessage = e.response.data })
    }
  }
}
</script>

<style>
.UpdateProfile{
  width: 70%;
  margin: 1px auto;
}
</style>