<template>
    <div>
        <h1>Weather forecast</h1>

        <p>This component demonstrates fetching data from the server.</p>

        <p v-if="!forecasts"><em>Loading...</em></p>

        <table class="table" v-if="forecasts">
            <thead>
                <tr>
                    <th>Date</th>
                    <th>Temp. (C)</th>
                    <th>Temp. (F)</th>
                    <th>Summary</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="forecast in forecasts" >
                    <td>{{ forecast.dateFormatted }}</td>
                    <td>{{ forecast.temperatureC }}</td>
                    <td>{{ forecast.temperatureF }}</td>
                    <td>{{ forecast.summary }}</td>
                </tr>
            </tbody>
        </table>

        <ul v-if="numbers" class="list">
            <li v-for="num in numbers" class="item">
                {{ num }}
            </li>
        </ul>
    </div>
</template>

<script>
export default {
    data() {
        return {
            forecasts: null,
            numbers: null
        }
    },

    methods: {
    },

    async created() {
        try {
            let response = await this.$http.get('/api/SampleData/WeatherForecasts')
            console.log(response.data);
            this.forecasts = response.data;
        } catch (error) {
            console.log(error)
        }

        try {
            let response = await this.$http.get('/api/SampleData/RandomNumbers')
            console.log(response.data);
            this.numbers = response.data;
        } catch (error) {
            console.log(error)
        }
    }
}
</script>

<style>
</style>