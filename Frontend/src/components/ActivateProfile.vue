<template>
  <div class="ActivateProfile">
    <div class="UpdateProfileFields" v-if="isLoggedIn.isLogin">
    <h1>Activate Profile</h1>

    <br />
    <v-alert
      :value="httpMessage"
      dismissible
      type="success"
      transition="scale-transition"
    >
    {{httpMessage}}
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
            v-model="firstName"
            type="text"
            label="First Name"
            :rules='fieldRules'/>
      <br />
      <v-text-field
            name="LastName"
            id="LastName"
            v-model="lastName"
            type="text"
            label="Last Name"
            :rules='fieldRules'/>
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
            v-model="DoB"
            label="Date of Birth"
            prepend-icon="event"
            readonly
            v-on="on"
            id="DoB"
            :rules='fieldRules'
          ></v-text-field>
        </template>
        <v-date-picker
          ref="picker"
          v-model="DoB"
          :max="new Date().toISOString().substr(0, 10)"
          min="1900-01-01"
          @change="updateDate"
        ></v-date-picker>
      </v-menu>
      <br />
      <v-text-field
            name="City"
            id="City"
            v-model="city"
            type="text"
            label="City"
            :rules='fieldRules'/>
      <br />
      <v-text-field
            name="State"
            id="State"
            v-model="state"
            type="text"
            label="State"
            :rules='fieldRules'/>
      <br />
      <v-text-field
            name="Country"
            id="Country"
            v-model="country"
            type="text"
            label="Country"
            :rules='fieldRules'/>
      <br />
      <v-checkbox
      v-model="checkbox"
      :rules="[v => !!v || 'You must agree to activate your profile.']"
      color="success"
      label="By checking this box you agree to our Terms and Conditions."
      required
    ></v-checkbox>
    <h5>
        Software is provided on an 'AS IS' basis, without warranty of any kind, including without limitation the warranties of
    merchantability, fitness for a particular purpose and non-infringement. The entire risk as to the quality and
    performance of the Software is borne by you. Should the Software prove defective, you and not the vendor assume the entire
    cost of any service and repair.
    In no event shall the creators, contributors, or providers be liable for any direct, indirect, incidental, special,
    exemplary, or consequential damages (including, but not limited to, procurement of substitute goods or services; loss of use,
    data, or profits; or business.
    </h5>
      <br>
      <v-btn color="success" v-on:click="ActivateProfile">Activate Profile</v-btn>
      </div>
  </div>
</template>

<script>
import axios from 'axios'
import { apiURL } from '@/const.js'
import { store } from '@/router/request'

export default {
  name: 'ActivateProfile',
  data () {
    return {
      checkbox: false,
      isLoggedIn: store.state,
      fieldRules: [v => !!v || 'This field is required'],
      menu: false,
      firstName: null,
      lastName: null,
      DoB: '',
      city: null,
      state: null,
      country: null,
      httpMessage: null,
      errorMessage: null,
      loggedInMessage: 'You must be logged in to update your profile'
    }
  },
  watch: {
    menu (val) {
      val && setTimeout(() => (this.$refs.picker.activePicker = 'YEAR'))
    }
  },
  methods: {
    updateDate (date) {
      this.$refs.menu.save(date)
    },
    ActivateProfile: function () {
      if (this.checkbox !== true) {
        this.errorMessage = 'You must agree to the Terms and Conditions to continue.'
      } else if (!this.firstName || !this.lastName || !this.DoB || !this.city || !this.state || !this.country) {
        this.errorMessage = 'Fields cannot be empty'
      } else {
        axios({
          method: 'POST',
          url: `${apiURL}` + '/profile/update',
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
          .then(response => (this.httpMessage = response.data))
          .catch(e => { this.errorMessage = e.response.data })
      }
    }
  }
}
</script>

<style>
.ActivateProfile{
  width: 70%;
  margin: 1px auto;
}
</style>
