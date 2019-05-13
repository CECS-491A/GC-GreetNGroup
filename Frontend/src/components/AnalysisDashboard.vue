<template>
  <div class="UserAnalytics">
    <h1 class='display-2'>Analysis Dashboard</h1>
    <div>
    <input v-model='month' type="text" :maxlength=15 placeholder="Search for a month">
    <input v-model='year' type="text" :maxlength=4 placeholder="Search for year">
    </div>
    <div>
    <button @click=" searchInput(month, year)">Search</button>
    <div v-bind:style="{ color: 'red', fontSize: 30 + 'px' }"> {{messageResults}}</div>
    <v-container fluid grid-list-md>
    <v-layout row wrap>
      <v-flex d-flex xs12 sm6 md4>
        <v-expansion-panel color="purple" style="maxWidth: 550px; text-align: center; margin: auto; font-size: 20px; font-weight: bold;">
          <v-expansion-panel-content style="background:purple;color:white">
            <template v-slot:header>
              <div>Login Compared To Registered Users</div>
            </template>
            <v-card color="purple" dark>
              <v-card-text>
                <li v-for="(value, index) in this.logvsreg"  v-bind:key="index" style = "list-style-type : none;  font-size: 14px;">
                <h1 v-if="index % 4 === 0"><u>{{value.Date}}</u><br /></h1>
                {{value.InfoType}} : {{value.Value}} 
                </li>
                </v-card-text>
              </v-card>
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-flex>
      <v-flex d-flex xs12 sm6 md4>
        <v-expansion-panel color="purple" style="maxWidth: 550px; text-align: center; margin: auto; font-size: 20px; font-weight: bold;">
          <v-expansion-panel-content style="background:purple;color:white">
            <template v-slot:header>
              <div>Session Time Information (minutes)</div>
            </template>
            <v-card color="purple" dark>
              <v-card-text>
                <li v-for="(value, index) in avgsession"  v-bind:key="index" style = "list-style-type : none;  font-size: 14px;">
                  <h1 v-if="index % 3 === 0"><u>{{value.Date}}</u><br /></h1>
                {{value.InfoType}} : {{value.Value}} </li>
                </v-card-text>
              </v-card>
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-flex>
      <v-flex d-flex xs12 sm6 md4>
        <v-expansion-panel color="purple" style="maxWidth: 550px; text-align: center; margin: auto; font-size: 20px; font-weight: bold;">
          <v-expansion-panel-content style="background:purple;color:white">
            <template v-slot:header>
            <div>Top 5 Most Used Features</div>
            </template>
            <v-card color="purple" dark>
                <v-card-text>
                  <li v-for="(value, index) in top5feature"  v-bind:key="index" style = "list-style-type : none;  font-size: 14px;">
                  <h1 v-if="index % 5 === 0"><u>{{value.Date}}</u><br /></h1>
                  {{value.InfoType}} : {{value.Value}} </li>
                  </v-card-text>
              </v-card>
          </v-expansion-panel-content>
        </v-expansion-panel>  
      </v-flex>
    </v-layout>
  </v-container>
   <v-container fluid grid-list-md>
    <v-layout row wrap>
      <v-flex d-flex xs12 sm6 md4>
        <v-expansion-panel color="purple" style="maxWidth: 550px; text-align: center; margin: auto; font-size: 20px; font-weight: bold;">
          <v-expansion-panel-content style="background:purple;color:white">
            <template v-slot:header>
            <div>Top 5 Pages (minutes)</div>
            </template>
            <v-card color="purple" dark>
                <v-card-text>
                  <li v-for="(value, index) in top5pages"  v-bind:key="index" style = "list-style-type : none;  font-size: 14px;">
                  <h1 v-if="index % 5 === 0"><u>{{value.Date}}</u><br /></h1>
                  {{value.InfoType}} : {{value.Value}} </li>
                  </v-card-text>
              </v-card>
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-flex>
      <v-flex d-flex xs12 sm6 md4>
        <v-expansion-panel color="purple" style="maxWidth: 550px; text-align: center; margin: auto; font-size: 20px; font-weight: bold;">
          <v-expansion-panel-content style="background:purple;color:white">
          <template v-slot:header>
          <div>Total Logins</div>
          </template>
          <v-card color="purple" dark>
              <v-card-text>
                <li v-for="(value, index) in loginmonthly"  v-bind:key="index" style = "list-style-type : none;  font-size: 14px;">
                <h1 v-if="index % 1 === 0"><u>{{value.Date}}</u><br /></h1>
                {{value.InfoType}} : {{value.Value}} </li>
              </v-card-text>
          </v-card>
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-flex>
      <v-flex d-flex xs12 sm6 md4>
        <v-expansion-panel color="purple" style="maxWidth: 550px; text-align: center; margin: auto; font-size: 20px; font-weight: bold;">
          <v-expansion-panel-content style="background:purple;color:white">
            <template v-slot:header>
              <div>Avg Session Time (minutes)</div>
            </template>
            <v-card color="purple" dark>
                <v-card-text>
                  <li v-for="(value, index) in sessionmonthly"  v-bind:key="index" style = "list-style-type : none;  font-size: 14px;">
                  <h1 v-if="index % 1 === 0"><u>{{value.Date}}</u><br /></h1>
                  {{value.InfoType}} : {{value.Value}} </li>
                </v-card-text>
            </v-card>
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-flex>
    </v-layout>
  </v-container>
  <v-container fluid grid-list-md>
    <v-layout row wrap>
      <v-flex d-flex xs12 sm6 md4>
         <v-expansion-panel color="purple" style="maxWidth: 550px; text-align: center; margin: auto; font-size: 20px; font-weight: bold;">
            <v-expansion-panel-content style="background:purple;color:white">
            <template v-slot:header>
              <div>Successful Login VS Failed Logins</div>
            </template>
            <v-card color="purple" dark>
                <v-card-text>
                  <li v-for="(value, index) in loginsuccessfail"  v-bind:key="index" style = "list-style-type : none;  font-size: 14px;">
                  <h1 v-if="index % 3 === 0"><u>{{value.Date}}</u><br /></h1>
                  {{value.InfoType}} : {{value.Value}} </li>
                  </v-card-text>
              </v-card>
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-flex>
    </v-layout>
  </v-container>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import { apiURL } from '@/const.js'
export default {
  name: 'UserAnalytics',
  data () {
    return {
      input: '',
      logvsreg: {},
      loginsuccessfail: {},
      avgsession: {},
      top5feature: {},
      top5pages: {},
      loginmonthly: {},
      sessionmonthly: {},
      month: '',
      year: '',
      messageResults: ''
    }
  },

  methods: {
    checkMonth: function (i) {
      var months = [ 'January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December' ]
      i = i.charAt(0).toUpperCase() + i.slice(1)
      if (months.indexOf(i) !== -1) {
        return true
      }
      return false
    },
    checkYear: function (i) {
      return (Number.isInteger(+i) && (+i > 0)) 
    },
    searchInput: function (month, year) {
      var checkMonth = this.checkMonth(month)
      var checkYear = this.checkYear(year)
      if (checkYear === true && checkMonth === true) {
        this.messageResults = 'Loading'
        month = month.charAt(0).toUpperCase() + month.slice(1)
        axios.all([
          this.LoginVsRegistered(month, year),
          this.AverageSessionDuration(month, year),
          this.Top5Features(month, year),
          this.Top5Pages(month, year),
          this.MonthlyLogin(month, year),
          this.MonthlySessionDuration(month, year),
          this.LoginSuccessFail(month, year)
        ])
          .then(axios.spread((firstResponse, secondResponse, thirdResponse, fourthResponse, fifthResponse, sixthResponse, seventhResponse) => {
            this.logvsreg = firstResponse.data
            this.avgsession = secondResponse.data
            this.top5feature = thirdResponse.data
            this.top5pages = fourthResponse.data
            this.loginmonthly = fifthResponse.data
            this.sessionmonthly = sixthResponse.data
            this.loginsuccessfail = seventhResponse.data
            this.messageResults = month + ' ' + year  
          }))
      } else {
        this.messageResults = 'Date not Valid'
      }
    },
    LoginVsRegistered (month, year) {
      return axios.get(`${apiURL}/UAD/LoginVSRegistered/` + month + '/' + year).catch(error => console.log(error))
    },
    AverageSessionDuration (month, year) {
      return axios.get(`${apiURL}/UAD/AverageSessionDuration/` + month + '/' + year).catch(error => console.log(error))
    },
    Top5Features (month, year) {
      return axios.get(`${apiURL}/UAD/Top5MostUsedFeature/` + month + '/' + year).catch(error => console.log(error))
    },
    Top5Pages (month, year) {
      return axios.get(`${apiURL}/UAD/Top5AveragePageSession/` + month + '/' + year).catch(error => console.log(error))
    },
    MonthlyLogin (month, year) {
      return axios.get(`${apiURL}/UAD/LoggedInMonthly/` + month + '/' + year).catch(error => console.log(error))
    },
    MonthlySessionDuration (month, year) {
      return axios.get(`${apiURL}/UAD/AverageSessionMonthly/` + month + '/' + year).catch(error => console.log(error))
    },
    LoginSuccessFail (month, year) {
      return axios.get(`${apiURL}/UAD/LoginSuccessFail/` + month + '/' + year).catch(error => console.log(error))
    }
  } 
    
}
</script>

<style>
h1{
  padding-top: 10px;
  padding-right: 20px;
  padding-bottom: 20px;
  padding-left: 20px;
  text-align: center;
}

ul {
  padding:0;
  margin:0;
  display: inline-block;
  text-align:left;
}
</style>
