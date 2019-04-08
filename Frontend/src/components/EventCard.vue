<template>
    <div class="card">
        <div>
            <h3 class="EventName"> {{ eventName | capitalize }} </h3>
        </div>
    </div>
</template>

<script>
export default {
    data() {
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
        };
    },
    filters: {

    },
    props: {

    }
};

</script>

<style>

</style>