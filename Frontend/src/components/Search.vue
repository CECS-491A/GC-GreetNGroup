<template>
    <div>
        <div class="Search">
            <h1>{{ title }}</h1>
        </div>
        <div>
            <input type="text" v-model="search" placeholder="search for events" />
        <button v-on:click="findEventsByName(search)">Search</button>
        <div id="events-list">
            <div v-if="events !== null">
                <div id="events" v-for="{UserId, User, EventId, StartDate, EventName, index} in events" :key="index">
                    <button id="event-b"> {{EventName}} </button>
                    <article> {{'Start Date: ' + StartDate}} </article>
                </div>
            </div>
        </div>
        </div>
    </div>
</template>

<script>
import axios from 'axios'

export default {
  name: 'SearchPage',
  data () {
    return {
      events: [],
      title: 'GreetNGroup',
      search: '',
      eventName: ''
    }
  },
  methods: {
    findEventsByName: function (i) {
      axios.get('http://localhost:62008/api/search/' + i)// build version -> 'https://api.greetngroup.com/api/search/' + i)
        .then((response) => { 
          const isDataAvailable = response.data && response.data.length; this.events = isDataAvailable ? response.data : []
        })
        .catch(error => console.log(error))
    }
  }
}
</script>

<style scoped>
h1, h2 {
    font-weight: normal;
}
#event-b {
    font-weight: bold;
    font-size: 20px;
}
#events-list {
    margin: 25px;
}
#events {
    min-height: 100px;
}
</style>
