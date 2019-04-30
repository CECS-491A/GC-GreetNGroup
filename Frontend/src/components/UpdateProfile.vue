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
            v-model="this.firstName"
            type="text"
            label="First Name"/>
      <br />
      <v-text-field
            name="LastName"
            id="LastName"
            v-model="this.lastName"
            type="text"
            label="Last Name"/>
      <br />
      <v-menu
        ref="menu"
        v-model="menu"
        :close-on-content-click="false"
        :nudge-right="40"
        lazy
        transition="scale-transition"
        offset-y
        full-width
        min-width="290px"
      >
        <template v-slot:activator="{ on }">
          <v-text-field
            v-model="dob"
            label="Date of Birth"
            prepend-icon="event"
            readonly
            v-on="on"
            id="dob"
          ></v-text-field>
        </template>
        <v-date-picker
          ref="picker"
          v-model="dob"
          :max="new Date().toISOString().substr(0, 10)"
          min="1900-01-01"
          @change="updateDate"
        ></v-date-picker>
      </v-menu>
      <br />
      <v-text-field
            name="City"
            id="City"
            v-model="this.city"
            type="text"
            label="City"/>
      <br />
      <v-text-field
            name="State"
            id="State"
            v-model="this.state"
            type="text"
            label="State"/>
      <br />
      <v-text-field
            name="Country"
            id="Country"
            v-model="this.country"
            type="text"
            label="Country"/>
      <br />
      <v-btn color="success" v-on:click="UpdateProfile">Update Profile</v-btn>
  </div>
</template>

<script>
import axios from 'axios'
import { apiURL } from '@/const.js'

export default {
  name: 'UpdateProfile',
  data () {
    return {
      firstName: null,
      lastName: null,
      DoB: null,
      city: null,
      state: null,
      country: null,
      message: null,
      errorMessage: null
    }
  },
  methods: {
    updateDate (date) {
      this.$refs.menu.save(date)
    },
    UpdateProfile: function () {
      axios({
        method: 'POST',
        url: `${apiURL}` + 'user/update',
        headers: {
          'Access-Control-Allow-Origin': '*',
          'Access-Control-Allow-Credentials': true
        },
        data: {
          FirstName: this.firstName,
          LastName: this.lastName,
          DoB: this.DoB,
          City: this.city,
          State: this.state,
          Country: this.country,
          JwtToken: localStorage.getItem('token')
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
