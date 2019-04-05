<template>
  <div class="NoPage">
    <h1>{{ msg }}</h1>
    <h2>{{ info }}</h2>
  </div>
</template>

<script>
export default {
  name: 'PageNotFound',
  data () {
    return {
      msg: 'Sorry! The page you are looking for could not be found.',
      info: null
    }
  }
}
</script>

<style scoped>
h1, h2 {
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
