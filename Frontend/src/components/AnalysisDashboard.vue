<template>
  <div class="UserAnalytics">
    <h1 class='display-2'>Analysis Dashboard</h1>
    <div>
      <v-layout align-start justify-center row wrap>
        <v-flex xs2>
            <v-select
              v-model="month"
              :items="months"
              :menu-props="{ maxHeight: '200' }"
              label="Select a Month"
            ></v-select>
            <v-select
              v-model="year"
              :items="years"
              :menu-props="{ maxHeight: '200' }"
              label="Select A Year"
            ></v-select>
            <v-btn v-on:click="searchInput(month, year)">Search</v-btn>
        </v-flex>
      </v-layout>
    </div>

    <div v-bind:style="{ color: 'black', fontSize: 30 + 'px' }"> {{messageResults}}</div>
      <v-expansion-panel
        v-model="panel"
        expand
        v-expansion-panel color="purple" 
        style="maxWidth: 800px; text-align: center; margin: auto; font-size: 20px; font-weight: bold;"
      >
        <v-expansion-panel-content
          v-for="(item, i) in 7"
          :key="i"
          style="background:purple;color:white"
        >
          <template v-slot:header>
            <div>{{titles[i]}}</div>
          </template>
          <v-card>
            <template v-if="i === 0">
              <li v-for="(value, index) in logvsreg"  v-bind:key="index" style = "list-style-type : none;  font-size: 18px;">
                  {{value.InfoType}} : {{value.Value}} 
                  </li>
            </template>
            <template v-if="i === 1">
              <li v-for="(value, index) in avgsession"  v-bind:key="index" style = "list-style-type : none;  font-size: 18px;">
                  {{value.InfoType}} : {{value.Value}} </li>
            </template>
            <template v-if="i === 2">
              <li v-for="(value, index) in top5feature"  v-bind:key="index" style = "list-style-type : none;  font-size: 18px;">
                    {{value.InfoType}} : {{value.Value}} </li>
            </template>
            <template v-if="i === 3">
              <li v-for="(value, index) in top5pages"  v-bind:key="index" style = "list-style-type : none;  font-size: 18px;">
                    {{value.InfoType}} : {{value.Value}} </li>
            </template>
            <template v-if="i === 4">
              <li v-for="(value, index) in loginmonthly"  v-bind:key="index" style = "list-style-type : none;  font-size: 18px;">
                  {{value.InfoType}} : {{value.Value}} </li>
            </template>
            <template v-if="i === 5">
              <li v-for="(value, index) in sessionmonthly"  v-bind:key="index" style = "list-style-type : none;  font-size: 18px;">
                    {{value.InfoType}} : {{value.Value}} </li>
            </template>
            <template v-if="i === 6">
              <li v-for="(value, index) in loginsuccessfail"  v-bind:key="index" style = "list-style-type : none;  font-size: 18px;">
                    {{value.InfoType}} : {{value.Value}} </li>
            </template>
          </v-card>
        </v-expansion-panel-content>
      </v-expansion-panel>
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
      messageResults: '',
      titles: ['Login vs Registered', 'Session Information (minutes)', 'Top 5 Most Used Features', 'Top 5 Pages (minutes)', 'Total Logins', 'Avg Session Time (minutes)', 'Successful Login VS Failed Logins'],
      selected: 'A',
      months: [
        { text: 'January', value: 'January' },
        { text: 'February', value: 'February' },
        { text: 'March', value: 'March' },
        { text: 'April', value: 'April' },
        { text: 'May', value: 'May' },
        { text: 'June', value: 'June' },
        { text: 'July', value: 'July' },
        { text: 'August', value: 'August' },
        { text: 'September', value: 'September' },
        { text: 'October', value: 'October' },
        { text: 'November', value: 'November' },
        { text: 'December', value: 'December' }
      ],
      years: [
        { text: '2019', value: 2019 },
        { text: '2018', value: 2018 },
        { text: '2017', value: 2017 },
        { text: '2016', value: 2016 },
        { text: '2015', value: 2015 },
        { text: '2014', value: 2014 }
      ],
      panel: [true, true, true, true, true, true, true]
    }
  },
  created () {
    axios.all([
      this.LoginVsRegistered('April', 2019),
      this.AverageSessionDuration('April', 2019),
      this.Top5Features('April', 2019),
      this.Top5Pages('April', 2019),
      this.MonthlyLogin('April', 2019),
      this.MonthlySessionDuration('April', 2019),
      this.LoginSuccessFail('April', 2019)
    ])
      .then(axios.spread((firstResponse, secondResponse, thirdResponse, fourthResponse, fifthResponse, sixthResponse, seventhResponse) => {
        this.logvsreg = firstResponse.data
        this.avgsession = secondResponse.data
        this.top5feature = thirdResponse.data
        this.top5pages = fourthResponse.data
        this.loginmonthly = fifthResponse.data
        this.sessionmonthly = sixthResponse.data
        this.loginsuccessfail = seventhResponse.data
        this.messageResults = 'April 2019'  
      }))
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
        this.messageResults = 'Invalid Month or Year'
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
  text-align: center;
}
</style>
