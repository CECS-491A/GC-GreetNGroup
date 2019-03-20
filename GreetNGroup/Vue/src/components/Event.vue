<template>
  <div>
    <div class="EventSearch">
        <h1>{{ title }}</h1>
        <h2>{{ info }}</h2>
    </div>
    <div>
        <input type="text" v-model="search" placeholder="search for events by id" />
        <button v-on:click="lookupEventById(search)">Search</button>
        <p>
          {{ eventName = eventInfo.EventName }}
        </p>
    </div>
  </div>
</template>

<script>
import axios from 'axios'

export default {
  name: 'EventHome',
  data () {
    return {
      eventInfo: {
        UserId: null,
        User: null,
        EventId: null,
        StartDate: null,
        EventName: null
      },
      title: 'GreetNGroup',
      info: null,
      search: '',
      eventName: ''
    }
  },
  methods: {
    lookupEventById: function (i) {
      axios.get('http://localhost:50884/api/Event/' + i)
        .then((response) => { this.eventInfo = response.data })
        .catch(error => console.log(error))
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h1, h2, h3 {
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

</style>
